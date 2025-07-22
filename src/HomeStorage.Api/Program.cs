using HomeStorage.Core;
using HomeStorage.Core.DTOs;
using HomeStorage.Core.Entities;
using HomeStorage.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCore(builder.Configuration);

var app = builder.Build();

app.UseCore();

app.MapGet("/products", ([FromServices] IProductRepository productRepository) =>
{
    var products = productRepository.GetAll()
        .Select(product => product.ToDto())
        .ToList();
    return Results.Ok(products);
}).WithName("GetProducts");

app.MapGet("/products/{id:guid}", ([FromServices] IProductRepository productRepository, Guid id) =>
{
    var product = productRepository.Get(id);
    
    return product is null ? Results.NotFound() : Results.Ok(product.ToDto());
}).WithName("GetProduct");

app.MapPost("/products", ([FromServices] IProductRepository productRepository, [FromBody] CreateProduct command) =>
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
        productRepository.Create(product);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        return Results.StatusCode(500);
    }
    
    return Results.CreatedAtRoute("GetProduct", new { id = (Guid)product.Id });
    
}).WithName("AddProduct");

app.MapPut("/products/{id:guid}", ([FromServices] IProductRepository productRepository, Guid id, UpdateProduct command) =>
{
    var product = productRepository.Get(id);
    
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
        productRepository.Update(product);
    }
    catch (Exception)
    {
        return Results.StatusCode(500);
    }
    
    return Results.Ok();
}).WithName("UpdateProduct");

app.MapDelete("/products/{id:guid}", ([FromServices] IProductRepository productRepository, Guid id) =>
{
    var product = productRepository.Get(id);

    if (product is null)
    {
        return Results.NotFound();
    }
    
    productRepository.Delete(product);
    
    return Results.Ok();
}).WithName("DeleteProduct");

app.Run();
