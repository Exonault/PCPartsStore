using PCPartsStore.Data;
using PCPartsStore.Entities;
using PCPartsStore.Repository.Interfaces;
using PCPartsStore.Services.Interfaces;

namespace PCPartsStore.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly IProductRepository _productRepository;
    private readonly IProductImageRepository _productImageRepository;

    private readonly ApplicationDbContext _applicationDbContext;

    public ProductService(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment,
        IProductImageRepository productImageRepository, IProductRepository productRepository)
    {
        _dbContext = dbContext;
        _webHostEnvironment = webHostEnvironment;
        _productImageRepository = productImageRepository;
        _productRepository = productRepository;
    }


    public async Task Add(Product product)
    {
        var productImage = _productImageRepository.GetProductImageByName(product.Image.FileName);

        product.ProductImage = productImage;

        await _productRepository.AddProduct(product);
    }

    public async Task<bool> UploadImage(IFormFile file, int? oldId)
    {
        if (oldId != null && !_productImageRepository.ProductImageExists(file.FileName))
        {
            var uploadPath = @$".\wwwroot\images\products\{file.FileName}";
            var filePath = @$"/images/products/{file.FileName}";

            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                var entity = _productImageRepository.GetProductImageById(oldId);
                entity.Name = file.FileName;
                entity.ImagePath = filePath;

                await _applicationDbContext.SaveChangesAsync();
            }

            return true;
        }
        
        if (oldId == null && _dbContext.ProductsImages.Any(i => i.Name == file.FileName) == false)
        {
            var modelId = _dbContext.ProductsImages.OrderByDescending(i => i.Id).FirstOrDefault();
            var id = 1;
            if (modelId != null) id = modelId.Id + 1;
            
            var uploadPath = @$".\wwwroot\images\products\{file.FileName}";
            var filePath = @$"/images/products/{file.FileName}";
            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                var imageModel = new ProductImage { Id = id, ImagePath = filePath, Name = file.FileName };
                await _dbContext.ProductsImages.AddAsync(imageModel);
                await _dbContext.SaveChangesAsync();
            }

            return true;
        }

        return false;
    }

    public Task<bool> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Edit(Product product, IFormFile image, int id)
    {
        throw new NotImplementedException();
    }

    public List<Product> GetProductsByCategory(int categoryId)
    {
        throw new NotImplementedException();
    }
}