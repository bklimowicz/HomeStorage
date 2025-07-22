using HomeStorage.Core.Entities;
using HomeStorage.Core.Repositories;
using HomeStorage.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace HomeStorage.Core.DAL.Repositories;

internal sealed class CosmosDbProductRepository : IProductRepository
{
    private readonly HomeStorageDbContext _dbContext;

    public CosmosDbProductRepository(HomeStorageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> GetAsync(ProductId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        return await _dbContext.Products.FirstAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _dbContext.Products.ToListAsync();
    } 

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