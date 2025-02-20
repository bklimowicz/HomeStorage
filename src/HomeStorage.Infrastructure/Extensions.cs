using HomeStorage.Domain.Repositories;
using HomeStorage.Infrastructure.DAL;
using HomeStorage.Infrastructure.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace HomeStorage.Infrastructure;

public static class Extensions
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        services.AddPostgress();
        services.AddLogging(options =>
        {
            options.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger());
        });
        services.AddOpenApi();
        services.AddTransient<IPostgresLocationRepository, PostgresLocationRepository>();
        services.AddTransient<IPostgresProductRepository, PostgresProductRepository>();        
    }
    
    public static void UseInfrastructure(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.UseHttpsRedirection();
    }
}