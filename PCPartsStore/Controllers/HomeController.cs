using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCPartsStore.Entities;
using PCPartsStore.Models;
using PCPartsStore.Repository.Interfaces;

namespace PCPartsStore.Controllers;

[Authorize(Policy = "FirstTimeSetupComplete")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IProductImageRepository _productImageRepository;

    public HomeController(ILogger<HomeController> logger, IProductRepository productRepository,
        IProductImageRepository productImageRepository, IProductCategoryRepository productCategoryRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
        _productImageRepository = productImageRepository;
        _productCategoryRepository = productCategoryRepository;
    }

    public async Task<IActionResult> Index()
    {
        var latestProducts = (_productRepository.GetLatestProducts(5)).ToList();

        ViewData["actions"] = (_productCategoryRepository.GetProductCategories())
            .Select(pc => pc.Name)
            .ToList();

        foreach (var product in latestProducts)
        {
            product.ProductImage = _productImageRepository.GetProductImageById(product.ProductImageId);
        }

        return View(latestProducts);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> Search(string searchString, int? page)
    {
        // ViewData["actions"] = await _productCategoryRepository.GetProductCategories();
        IEnumerable<ProductCategory> productCategories = _productCategoryRepository.GetProductCategories();

        List<string> productCategoriesNames = productCategories.Select(x => x.Name).ToList();
        
        ViewData["actions"] = productCategoriesNames;
        // search
        
        return View();
    }
}