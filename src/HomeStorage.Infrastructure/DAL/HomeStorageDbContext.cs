using HomeStorage.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeStorage.Infrastructure.DAL;

internal sealed class HomeStorageDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Location> Locations { get; set; }

    public HomeStorageDbContext(DbContextOptions<HomeStorageDbContext> dbContextOptions) : base(dbContextOptions)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}