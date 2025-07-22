using HomeStorage.Core;
using HomeStorage.Core.DTOs;
using HomeStorage.Core.Entities;
using HomeStorage.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCore(builder.Configuration);

var app = builder.Build();

app.UseCore();

app.MapGet("/products", async ([FromServices] IProductRepository productRepository) =>
{
    var products = (await productRepository.GetAll())
        .Select(product => product.ToDto())
        .ToList();
    return Results.Ok(products);
}).WithName("GetProducts");

app.MapGet("/products/{id:guid}", async ([FromServices] IProductRepository productRepository, Guid id) =>
{
    var product = await productRepository.Get(id);
    
    return product is null ? Results.NotFound() : Results.Ok(product.ToDto());
}).WithName("GetProduct");

app.MapPost("/products", async ([FromServices] IProductRepository productRepository, [FromBody] CreateProduct command) =>
{
    var product = Product.Create(
        Guid.NewGuid(),
        command.Name,
        command.Quantity,
        command.Description!,
        command.Producer!
    );

    try
    {
        await productRepository.Create(product);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        return Results.StatusCode(500);
    }
    
    return Results.CreatedAtRoute("GetProduct", new { id = (Guid)product.Id });
    
}).WithName("AddProduct");

app.MapPut("/products/{id:guid}", async ([FromServices] IProductRepository productRepository, Guid id, UpdateProduct command) =>
{
    var product = await productRepository.Get(id);
    
    if (product is null)
    {
        return Results.NotFound();
    }
    
    product.UpdateName(command.Name);
    product.UpdateQuantity(command.Quantity);
    product.Description = command.Description!;
    product.Producer = command.Producer!;

    try
    {
        await productRepository.Update(product);
    }
    catch (Exception)
    {
        return Results.StatusCode(500);
    }
    
    return Results.Ok();
}).WithName("UpdateProduct");

app.MapDelete("/products/{id:guid}", async ([FromServices] IProductRepository productRepository, Guid id) =>
{
    var product = await productRepository.Get(id);

    if (product is null)
    {
        return Results.NotFound();
    }
    
    await productRepository.Delete(product);
    
    return Results.Ok();
}).WithName("DeleteProduct");

app.Run();
