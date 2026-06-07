using HomeStorage.Core.Entities;

namespace HomeStorage.Core.Repositories;

public interface ILocationRepository
{
    public Task<Location?> GetAsync(int id);
    public Task<IEnumerable<Location>> GetAllAsync();
    public Task<bool> ExistsAsync(int id);
    public Task<bool> HasProductsAsync(int id);
    public Task CreateAsync(Location location);
    public Task UpdateAsync(Location location);
    public Task DeleteAsync(Location location);
}
