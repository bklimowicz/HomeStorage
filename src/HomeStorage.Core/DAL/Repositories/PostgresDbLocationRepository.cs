using HomeStorage.Core.Entities;
using HomeStorage.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HomeStorage.Core.DAL.Repositories;

public class PostgresDbLocationRepository : ILocationRepository
{
    private readonly HomeStorageDbContext _dbContext;

    public PostgresDbLocationRepository(HomeStorageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Location?> GetAsync(int id) => await _dbContext.Locations.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<Location>> GetAllAsync() => await _dbContext.Locations.ToListAsync();

    public async Task<bool> ExistsAsync(int id) => await _dbContext.Locations.AnyAsync(x => x.Id == id);

    public async Task<bool> HasProductsAsync(int id) => await _dbContext.Products.AnyAsync(x => x.LocationId == id);

    public async Task CreateAsync(Location location)
    {
        _dbContext.Locations.Add(location);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Location location)
    {
        _dbContext.Locations.Update(location);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Location location)
    {
        _dbContext.Locations.Remove(location);
        await _dbContext.SaveChangesAsync();
    }
}
