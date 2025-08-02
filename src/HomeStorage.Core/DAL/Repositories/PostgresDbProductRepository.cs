using HomeStorage.Core.Entities;
using HomeStorage.Core.Repositories;
using HomeStorage.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace HomeStorage.Core.DAL.Repositories;

public class PostgresDbProductRepository : IProductRepository
{
    private readonly HomeStorageDbContext _dbContext;

    public PostgresDbProductRepository(HomeStorageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> GetAsync(ProductId id) => await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<Product>> GetAllAsync() => await _dbContext.Products.ToListAsync();

    public async Task CreateAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
    }
}