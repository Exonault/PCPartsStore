using Microsoft.EntityFrameworkCore;
using PCPartsStore.Data;
using PCPartsStore.Entities;
using PCPartsStore.Repository.Interfaces;

namespace PCPartsStore.Repository;

public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductCategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddProductCategory(ProductCategory productCategory)
    {
        await _dbContext.ProductsCategory.AddAsync(productCategory);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProductCategory>> GetProductCategories()
    {
        return await _dbContext.ProductsCategory.ToListAsync();
    }

    public async Task<ProductCategory?> GetProductCategoryById(int id)
    {
        return await _dbContext.ProductsCategory.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateProductCategory(ProductCategory productCategory)
    {
        _dbContext.Entry(productCategory).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProductCategory(ProductCategory productCategory)
    {
        _dbContext.ProductsCategory.Remove(productCategory);
        await _dbContext.SaveChangesAsync();
    }
}