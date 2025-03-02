using PCPartsStore.Entities;

namespace PCPartsStore.Repository.Interfaces;

public interface IProductCategoryRepository
{
    Task AddProductCategory(ProductCategory productCategory);

    Task<IEnumerable<ProductCategory>> GetProductCategories();

    Task<ProductCategory?> GetProductCategoryById(int id);

    Task UpdateProductCategory(ProductCategory productCategory);

    Task DeleteProductCategory(ProductCategory productCategory);
}