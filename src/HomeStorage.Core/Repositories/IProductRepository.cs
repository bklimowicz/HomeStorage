using HomeStorage.Core.Entities;
using HomeStorage.Core.ValueObjects;

namespace HomeStorage.Core.Repositories;

public interface IProductRepository
{
    public Task<Product?> Get(ProductId id);
    public Task<IEnumerable<Product>> GetAll();
    public Task Create(Product location);
    public Task Update(Product loTcation);
    public Task Delete(Product product);
}