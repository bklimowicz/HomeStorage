using HomeStorage.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeStorage.Infrastructure.DAL;

public sealed class HomeStorageDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Location> Locations { get; set; }
    
    public string DbPath { get; }

    public HomeStorageDbContext(DbContextOptions<HomeStorageDbContext> dbContextOptions) : base(dbContextOptions)
    {
        var path = Environment.CurrentDirectory;
        DbPath = Path.Join(path, "db/HomeStorage.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}