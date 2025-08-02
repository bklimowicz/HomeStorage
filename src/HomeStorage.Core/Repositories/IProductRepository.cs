using HomeStorage.Core.Entities;
using HomeStorage.Core.ValueObjects;

namespace HomeStorage.Core.Repositories;

public interface IProductRepository
{
    public Task<Product?> GetAsync(ProductId id);
    public Task<IEnumerable<Product>> GetAllAsync();
    public Task CreateAsync(Product product);
    public Task UpdateAsync(Product product);
    public Task DeleteAsync(Product product);
}