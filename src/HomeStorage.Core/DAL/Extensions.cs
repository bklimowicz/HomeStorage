using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeStorage.Core.DAL;

internal static class Extensions
{
    public static IServiceCollection AddCosmosDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HomeStorageDbContext>(x =>
        {
            var key = configuration["CosmosDb:Key"];
            x.UseCosmos("https://szkcosmosdb.documents.azure.com:443/",
                key!,
                "HomeStorage");
        });
        
        return services;
    }
}