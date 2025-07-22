using HomeStorage.Core.Entities;
using HomeStorage.Core.ValueObjects;

namespace HomeStorage.Core.Repositories;

public interface IProductRepository
{
    public Product? Get(ProductId id);
    public IEnumerable<Product> GetAll();
    public void Create(Product location);
    public void Update(Product location);
    public void Delete(Product product);
}