using PCPartsStore.Entities;

namespace PCPartsStore.Repository.Interfaces;

public interface IProductRepository
{
    Task AddProduct(Product product);

    Task<IEnumerable<Product>> GetProducts();
    
    bool ContainsProductWithId(int? id);
    
    Task<IEnumerable<Product>> GetLatestProducts(int count);

    Task<IEnumerable<Product>> GetProductsByCategory(int categoryId);

    Task<Product?> GetProductById(int id);

    Task UpdateProduct(Product product);

    Task DeleteProduct(Product product);
}