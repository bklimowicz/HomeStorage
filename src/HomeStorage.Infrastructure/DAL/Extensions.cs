using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HomeStorage.Infrastructure.DAL;

internal static class Extensions
{
    public static IServiceCollection AddPostgress(this IServiceCollection services)
    {
        const string connectionString = "Host=localhost;Port=5432;Database=HomeStorage;Username=postgres;Password=";
        services.AddDbContext<HomeStorageDbContext>(x =>
            x.UseNpgsql(connectionString));

        return services;
    }
}