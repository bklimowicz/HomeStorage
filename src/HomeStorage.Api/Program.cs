using HomeStorage.Infrastructure;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddExtensions();

var app = builder.Build();

app.UseExtensions();

app.MapGet("/hello", ([FromServices]ILogger<Program> logger) =>
    {
        logger.LogDebug("Hello endpoint was called");
        logger.LogInformation("Hello info");
        return "hello";
    })
.WithName("TestEndpoint");

//app.MapGet("/products", () => )

app.Run();
