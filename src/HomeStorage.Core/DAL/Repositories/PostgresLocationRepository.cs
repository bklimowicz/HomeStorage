using HomeStorage.Core.Entities;
using HomeStorage.Core.Repositories;
using HomeStorage.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace HomeStorage.Core.DAL.Repositories;

internal sealed class PostgresLocationRepository : IPostgresLocationRepository
{
    private readonly HomeStorageDbContext _dbContext;

    public PostgresLocationRepository(HomeStorageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Location Get(LocationName locationName)
        => _dbContext.Locations.Include(x => x.Products)
            .SingleOrDefault(x => x.LocationName == locationName)!;

    public IEnumerable<Location> GetAll()
        => _dbContext.Locations.Include(x => x.Products).ToList();

    public void Add(Location location)
    {
        _dbContext.Locations.Add(location);
        _dbContext.SaveChanges();
    }

    public void Update(Location location)
    {
        _dbContext.Locations.Update(location);
        _dbContext.SaveChanges();
    }

    public void Delete(Location location)
    {
        _dbContext.Locations.Remove(location);
        _dbContext.SaveChanges();
    }
}