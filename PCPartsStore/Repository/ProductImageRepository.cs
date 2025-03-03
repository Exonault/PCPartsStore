using Microsoft.EntityFrameworkCore;
using PCPartsStore.Data;
using PCPartsStore.Entities;
using PCPartsStore.Repository.Interfaces;

namespace PCPartsStore.Repository;

public class ProductImageRepository : IProductImageRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductImageRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task AddProductImage(ProductImage productImage)
    {
        await _dbContext.AddAsync(productImage);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProductImage>> GetProductImages()
    {
        return await _dbContext.ProductsImages.ToListAsync();
    }

    public async Task<ProductImage?> GetProductImageById(int productId)
    {
       return await _dbContext.ProductsImages.FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task UpdateProductImage(ProductImage productImage)
    {
        _dbContext.Entry(productImage).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProductImage(ProductImage productImage)
    {
        _dbContext.ProductsImages.Remove(productImage);
        await _dbContext.SaveChangesAsync();
    }
}