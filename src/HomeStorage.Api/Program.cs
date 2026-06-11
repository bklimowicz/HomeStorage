using HomeStorage.Api.Middleware;
using HomeStorage.Core;
using HomeStorage.Core.DAL;
using HomeStorage.Core.DTOs;
using HomeStorage.Core.Entities;
using HomeStorage.Core.Repositories;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddCore();

// Honour X-Forwarded-* when running behind the Cloudflare Tunnel (e.g. the
// future iOS app calling the API through a public hostname).
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCore();
app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HomeStorageDbContext>();
    await dbContext.Database.MigrateAsync();
}

// ---- Products ----

app.MapGet("/products", async ([FromServices] IProductRepository productRepository) =>
{
    var products = (await productRepository.GetAllAsync())
        .Select(product => product.ToDto())
        .ToList();
    return Results.Ok(products);
}).WithName("GetProducts");

app.MapGet("/products/{id:guid}", async ([FromServices] IProductRepository productRepository, Guid id) =>
{
    var product = await productRepository.GetAsync(id);

    return product is null ? Results.NotFound() : Results.Ok(product.ToDto());
}).WithName("GetProduct");

app.MapPost("/products", async (
    [FromServices] IProductRepository productRepository,
    [FromServices] ILocationRepository locationRepository,
    [FromBody] CreateProduct command) =>
{
    if (!await locationRepository.ExistsAsync(command.LocationId))
    {
        return Results.BadRequest(new { message = $"Location {command.LocationId} does not exist." });
    }

    var product = Product.Create(
        Guid.NewGuid(),
        command.Name,
        command.Quantity,
        command.LocationId,
        command.Description!,
        command.Producer!
    );

    await productRepository.CreateAsync(product);

    return Results.CreatedAtRoute("GetProduct", new { id = (Guid)product.Id });
}).WithName("AddProduct");

app.MapPut("/products/{id:guid}", async (
    [FromServices] IProductRepository productRepository,
    [FromServices] ILocationRepository locationRepository,
    Guid id,
    UpdateProduct command) =>
{
    var product = await productRepository.GetAsync(id);

    if (product is null)
    {
        return Results.NotFound();
    }

    if (!await locationRepository.ExistsAsync(command.LocationId))
    {
        return Results.BadRequest(new { message = $"Location {command.LocationId} does not exist." });
    }

    product.UpdateName(command.Name);
    product.UpdateQuantity(command.Quantity);
    product.MoveToLocation(command.LocationId);
    product.Description = command.Description!;
    product.Producer = command.Producer!;

    await productRepository.UpdateAsync(product);

    return Results.Ok();
}).WithName("UpdateProduct");

app.MapDelete("/products/{id:guid}", async ([FromServices] IProductRepository productRepository, Guid id) =>
{
    var product = await productRepository.GetAsync(id);

    if (product is null)
    {
        return Results.NotFound();
    }

    await productRepository.DeleteAsync(product);

    return Results.Ok();
}).WithName("DeleteProduct");

// ---- Locations ----

app.MapGet("/locations", async ([FromServices] ILocationRepository locationRepository) =>
{
    var locations = (await locationRepository.GetAllAsync())
        .Select(location => location.ToDto())
        .ToList();
    return Results.Ok(locations);
}).WithName("GetLocations");

app.MapGet("/locations/{id:int}", async ([FromServices] ILocationRepository locationRepository, int id) =>
{
    var location = await locationRepository.GetAsync(id);

    return location is null ? Results.NotFound() : Results.Ok(location.ToDto());
}).WithName("GetLocation");

app.MapPost("/locations", async ([FromServices] ILocationRepository locationRepository, [FromBody] CreateLocation command) =>
{
    var location = Location.Create(command.LocationName);

    await locationRepository.CreateAsync(location);

    return Results.CreatedAtRoute("GetLocation", new { id = location.Id }, location.ToDto());
}).WithName("AddLocation");

app.MapPut("/locations/{id:int}", async ([FromServices] ILocationRepository locationRepository, int id, UpdateLocation command) =>
{
    var location = await locationRepository.GetAsync(id);

    if (location is null)
    {
        return Results.NotFound();
    }

    location.UpdateName(command.LocationName);

    await locationRepository.UpdateAsync(location);

    return Results.Ok(location.ToDto());
}).WithName("UpdateLocation");

app.MapDelete("/locations/{id:int}", async ([FromServices] ILocationRepository locationRepository, int id) =>
{
    var location = await locationRepository.GetAsync(id);

    if (location is null)
    {
        return Results.NotFound();
    }

    if (await locationRepository.HasProductsAsync(id))
    {
        return Results.Conflict(new { message = "Location still has products assigned and cannot be deleted." });
    }

    await locationRepository.DeleteAsync(location);

    return Results.Ok();
}).WithName("DeleteLocation");

app.Run();
