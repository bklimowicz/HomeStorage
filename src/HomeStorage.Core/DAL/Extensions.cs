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
            var connectionString = configuration["ConnectionStrings:default"];
            x.UseSqlServer(connectionString);
        });
        
        return services;
    }
}