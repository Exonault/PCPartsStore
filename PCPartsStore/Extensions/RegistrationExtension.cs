using PCPartsStore.Repository;
using PCPartsStore.Repository.Interfaces;

namespace PCPartsStore.Extensions;

public static class RegistrationExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IProductImageRepository, ProductImageRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
    }
}