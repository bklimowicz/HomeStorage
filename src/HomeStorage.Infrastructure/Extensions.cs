using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace HomeStorage.Infrastructure;

public static class Extensions
{
    public static void AddExtensions(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        services.AddLogging(options =>
        {
            options.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger());
        });
        services.AddOpenApi();
    }
    
    public static void UseExtensions(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.UseHttpsRedirection();
    }
}