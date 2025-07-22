using HomeStorage.Core.Entities;
using HomeStorage.Core.Repositories;
using HomeStorage.Core.ValueObjects;

namespace HomeStorage.Core.DAL.Repositories;

public class CosmosDbProductRepository : IProductRepository
{
    public Product? Get(ProductId id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product> GetAll()
    {
        throw new NotImplementedException();
    }

    public void Create(Product location)
    {
        throw new NotImplementedException();
    }

    public void Update(Product location)
    {
        throw new NotImplementedException();
    }

    public void Delete(Product product)
    {
        throw new NotImplementedException();
    }
}