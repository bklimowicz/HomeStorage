using HomeStorage.Core;
using HomeStorage.Core.DTOs;
using HomeStorage.Core.Entities;
using HomeStorage.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCore();

var app = builder.Build();

app.UseCore();

app.MapGet("/products", ([FromServices] IPostgresProductRepository postgresProductRepository) =>
{
    var products = postgresProductRepository.GetAll()
        .Select(product => product.ToDto())
        .ToList();
    return Results.Ok(products);
}).WithName("GetProducts");

app.MapGet("/products/{id:guid}", ([FromServices] IPostgresProductRepository postgresProductRepository, Guid id) =>
{
    var product = postgresProductRepository.Get(id);
    
    return product is null ? Results.NotFound() : Results.Ok(product.ToDto());
}).WithName("GetProduct");

app.MapPost("/products", ([FromServices] IPostgresProductRepository postgresProductRepository, [FromBody] CreateProduct command) =>
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
        postgresProductRepository.Create(product);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        return Results.StatusCode(500);
    }
    
    return Results.CreatedAtRoute("GetProduct", new { id = (Guid)product.Id });
    
}).WithName("AddProduct");

app.MapPut("/products/{id:guid}", ([FromServices] IPostgresProductRepository postgresProductRepository, Guid id, UpdateProduct command) =>
{
    var product = postgresProductRepository.Get(id);
    
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
        postgresProductRepository.Update(product);
    }
    catch (Exception)
    {
        return Results.StatusCode(500);
    }
    
    return Results.Ok();
}).WithName("UpdateProduct");

app.MapDelete("/products/{id:guid}", ([FromServices] IPostgresProductRepository postgresProductRepository, Guid id) =>
{
    var product = postgresProductRepository.Get(id);

    if (product is null)
    {
        return Results.NotFound();
    }
    
    postgresProductRepository.Delete(product);
    
    return Results.Ok();
}).WithName("DeleteProduct");

app.Run();
