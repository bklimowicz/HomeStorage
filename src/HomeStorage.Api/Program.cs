using HomeStorage.Application;
using HomeStorage.Application.Commands;
using HomeStorage.Application.Commands.Handlers;
using HomeStorage.Domain.Entities;
using HomeStorage.Domain.Repositories;
using HomeStorage.Infrastructure;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddInfrastructure();
builder.Services.AddApplication();

var app = builder.Build();

app.UseInfrastructure();

app.MapGet("/hello", ([FromServices]ILogger<Program> logger) =>
    {
        logger.LogDebug("Hello endpoint was called");
        logger.LogInformation("Hello info");
        return "hello";
    })
.WithName("TestEndpoint");

app.MapGet("/products", ([FromServices] IPostgresProductRepository repository) =>
{
    var products = repository.GetAll();
    return Results.Ok(products);
});

app.MapGet("/products/{id:guid}", ([FromServices] IPostgresProductRepository repository, Guid id) =>
{
    var product = repository.Get(id);
    
    if (product is null)
    {
        return Results.NotFound();
    }
    
    return Results.Ok(product);
});

app.MapPost("/products/add", ([FromServices] ICommandHandler<AddProductCommand> commandHandler, AddProductCommand productCommand) =>
{
    if (productCommand.Id == Guid.Empty)
    {
        // var productCommand with {
        //     Id = Guid.NewGuid();
        // };
    }
    
    commandHandler.Handle(productCommand);

    return Results.CreatedAtRoute("/products/{id:guid}", new { id = productCommand.Id });
});

// #region Locations
// app.MapPost("/locations", ([FromServices]Postgres ))

app.Run();
