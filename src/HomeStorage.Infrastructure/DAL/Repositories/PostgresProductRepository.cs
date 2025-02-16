using HomeStorage.Domain.Entities;
using HomeStorage.Domain.Repositories;
using HomeStorage.Domain.ValueObjects;

namespace HomeStorage.Infrastructure.DAL.Repositories;

internal sealed class PostgresProductRepository : IPostgresProductRepository
{
    private readonly HomeStorageDbContext _dbContext;

    public PostgresProductRepository(HomeStorageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Product Get(ProductId id) 
        => _dbContext.Products.SingleOrDefault(x => x.Id == id)!;

    public IEnumerable<Product> GetAll() => _dbContext.Products.ToList();

    public void Add(Product location)
    {
        _dbContext.Products.Add(location);
        _dbContext.SaveChanges();
    }

    public void Update(Product location)
    {
        _dbContext.Products.Update(location);
        _dbContext.SaveChanges();
    }

    public void Delete(Product location)
    {
        _dbContext.Products.Remove(location);
        _dbContext.SaveChanges();
    }
}