using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HomeStorage.Core.DAL;

internal static class Extensions
{
    public static IServiceCollection AddPostgress(this IServiceCollection services)
    {
        const string connectionString = "Host=localhost;Port=5432;Database=HomeStorage;Username=postgres;Password=postgres";
        services.AddDbContext<HomeStorageDbContext>(x =>
            x.UseNpgsql(connectionString));
        
        services.AddDbContext<HomeStorageDbContext>();

        return services;
    }
}