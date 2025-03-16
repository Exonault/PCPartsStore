using PCPartsStore.Entities;

namespace PCPartsStore.Repository.Interfaces;

public interface IProductImageRepository
{
    Task AddProductImage(ProductImage productImage);

    Task<IEnumerable<ProductImage>> GetProductImages();
    
    ProductImage? GetProductImageById(int? productId);
    
    ProductImage? GetProductImageByName(string fileName);
    
    bool ProductImageExists(string fileName);

    Task UpdateProductImage(ProductImage productImage);

    Task DeleteProductImage(ProductImage productImage);
}