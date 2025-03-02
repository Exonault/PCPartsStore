using PCPartsStore.Entities;

namespace PCPartsStore.Repository.Interfaces;

public interface IProductImageRepository
{
    Task AddProductImage(ProductImage productImage);

    Task<IEnumerable<ProductImage>> GetProductImages();

    Task UpdateProductImage(ProductImage productImage);

    Task DeleteProductImage(ProductImage productImage);
}