using HomeStorage.Core;
using HomeStorage.Infrastructure.DAL;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCore();

var app = builder.Build();

app.UseCore();

app.MapGet("/test", ([FromServices] HomeStorageDbContext dbContext) =>
    {
        var test = dbContext.Products;
        return Results.Ok(dbContext.Products);
    })
    .WithName("Test");


app.MapGet("/products", ([FromServices] HomeStorageDbContext dbContext) =>
{
    return Results.Ok(dbContext.Products);
}).WithName("GetProducts");

app.MapGet("/products/{id:guid}", ([FromServices] HomeStorageDbContext dbContext, Guid id) =>
{
    return Results.Ok(dbContext.Products.FirstOrDefault(x => id == (Guid)x.Id));
}).WithName("GetProduct");

app.MapPost("/products/add", ([FromServices] HomeStorageDbContext dbContext) =>
{
    
}).WithName("AddProduct");

app.MapDelete("/products/{id:guid}", ([FromServices] HomeStorageDbContext dbContext) =>
{
    
}).WithName("DeleteProduct");

app.MapPut("/products/{id:guid}", ([FromServices] HomeStorageDbContext dbContext) =>
{
    
}).WithName("UpdateProduct");

app.Run();
