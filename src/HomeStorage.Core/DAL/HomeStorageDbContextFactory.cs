using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HomeStorage.Core.DAL;

// Used only by the EF Core CLI tooling (e.g. `dotnet ef migrations add`).
// At runtime the DbContext is configured by the Aspire Npgsql component.
internal sealed class HomeStorageDbContextFactory : IDesignTimeDbContextFactory<HomeStorageDbContext>
{
    public HomeStorageDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<HomeStorageDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=homestorage;Username=postgres;Password=postgres")
            .Options;

        return new HomeStorageDbContext(options);
    }
}
