using HomeStorage.Core.Entities;
using HomeStorage.Core.Repositories;
using HomeStorage.Core.ValueObjects;

namespace HomeStorage.Core.DAL.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly HomeStorageDbContext _dbContext;

    public ProductRepository(HomeStorageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Product? Get(ProductId id) 
        => _dbContext.Products.SingleOrDefault(x => x.Id == id);

    public IEnumerable<Product> GetAll() => _dbContext.Products.ToList();

    public void Create(Product product)
    {
        _dbContext.Products.Add(product);
        _dbContext.SaveChanges();
    }

    public void Update(Product product)
    {
        _dbContext.Products.Update(product);
        _dbContext.SaveChanges();
    }

    public void Delete(Product product)
    {
        _dbContext.Products.Remove(product);
        _dbContext.SaveChanges();
    }
}