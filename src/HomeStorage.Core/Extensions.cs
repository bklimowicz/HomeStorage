using System.Configuration;
using HomeStorage.Core.DAL;
using HomeStorage.Core.DAL.Repositories;
using HomeStorage.Core.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeStorage.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCosmosDb(configuration);
        services.AddOpenApi();
        services.AddScoped<IProductRepository, ProductRepository>();
        
        return services;
    }

    public static void UseCore(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.UseHttpsRedirection();
    }
}