using HomeStorage.Domain.Entities;
using HomeStorage.Domain.ValueObjects;

namespace HomeStorage.Domain.Repositories;

public interface IPostgresProductRepository
{
    public Product Get(ProductId id);
    public IEnumerable<Product> GetAll();
    public void Add(Product location);
    public void Update(Product location);
    public void Delete(Product location);
}