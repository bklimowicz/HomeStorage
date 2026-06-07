using HomeStorage.Core.DAL.Repositories;
using HomeStorage.Core.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeStorage.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddScoped<IProductRepository, PostgresDbProductRepository>();
        services.AddScoped<ILocationRepository, PostgresDbLocationRepository>();

        return services;
    }

    public static void UseCore(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
    }
}
