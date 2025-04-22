using HomeStorage.Core;
using HomeStorage.Core.Commands;
using HomeStorage.Domain.Entities;
using HomeStorage.Infrastructure.DAL;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCore();

var app = builder.Build();

app.UseCore();

var group = app.MapGroup("/api/v1").WithTags("v1");
var productGroup = group.MapGroup("/products").WithTags("Products");

app.MapGet("/test", ([FromServices] HomeStorageDbContext dbContext) =>
    {
        var test = dbContext.Products;
        return Results.Ok(dbContext.Products);
    })
    .WithName("Test");

productGroup.MapGet("/", ([FromServices] HomeStorageDbContext dbContext) =>
{
    var products = dbContext.Products.ToList();
    return Results.Ok(products);
}).WithName("GetProducts");

productGroup.MapGet("/{id:guid}", ([FromServices] HomeStorageDbContext dbContext, Guid id) =>
{
    var product = dbContext.Products.AsEnumerable().FirstOrDefault(x => id == x.Id.Value);
    
    return product is null ? Results.BadRequest() : Results.Ok(product);
}).WithName("GetProduct");

productGroup.MapPost("/", async ([FromServices] HomeStorageDbContext dbContext, AddProductCommand command) =>
{
    var product = Product.Create(Guid.NewGuid(), command.Name, command.Quantity, command.Description, command.Producer);
    
    dbContext.Products.Add(product);
    await dbContext.SaveChangesAsync();
    
    return Results.CreatedAtRoute("GetProduct", new { id = product.Id.Value }, product);

}).WithName("AddProduct");

productGroup.MapDelete("/{id:guid}", async ([FromServices] HomeStorageDbContext dbContext, Guid id) =>
{
    var product = dbContext.Products.AsEnumerable().FirstOrDefault(x => id == x.Id.Value);
    
    if (product is null)
    {
        return Results.NotFound();
    }
    dbContext.Products.Remove(product);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
}).WithName("DeleteProduct");

productGroup.MapDelete("/", async ([FromServices] HomeStorageDbContext dbContext) =>
{
    var products = dbContext.Products.ToList();
    dbContext.Products.RemoveRange(products);
    await dbContext.SaveChangesAsync();
    
    return Results.NoContent();
}).WithName("DeleteAllProducts");

productGroup.MapPut("/{id:guid}", async ([FromServices] HomeStorageDbContext dbContext, Guid id, UpdateOrAddProductCommand command) =>
{
    var product = dbContext.Products.AsEnumerable().FirstOrDefault(x => id == x.Id.Value);
    if (product is not null)
    {
        product.UpdateName(command.Name);
        product.UpdateQuantity(command.Quantity);
        
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    }
    else
    {
        var newProduct = new Product(Guid.NewGuid(), command.Name, command.Quantity, command.Description, command.Producer);
        dbContext.Products.Add(newProduct);
        await dbContext.SaveChangesAsync();
        return Results.CreatedAtRoute("GetProduct", new { id = newProduct.Id.Value }, newProduct);
    }
    
}).WithName("UpdateProduct");


app.Run();
