using HomeStorage.Core.DAL;
using HomeStorage.Core.DAL.Repositories;
using HomeStorage.Core.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeStorage.Core;

public static class Extensions
{
    /// <summary>
    /// Registers everything the Core layer provides: the EF Core
    /// <see cref="HomeStorageDbContext"/> (wired to the "homestorage" connection via the
    /// Aspire Npgsql component), the repository implementations and the API surface (OpenAPI).
    /// </summary>
    public static IHostApplicationBuilder AddCore(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<HomeStorageDbContext>("homestorage");

        builder.Services.AddScoped<IProductRepository, PostgresDbProductRepository>();
        builder.Services.AddScoped<ILocationRepository, PostgresDbLocationRepository>();

        builder.Services.AddOpenApi();

        return builder;
    }

    /// <summary>
    /// Wires the Core middleware and endpoints into the request pipeline.
    /// </summary>
    public static WebApplication UseCore(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        return app;
    }
}
