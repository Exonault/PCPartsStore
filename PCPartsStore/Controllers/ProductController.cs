using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PCPartsStore.Data;
using PCPartsStore.Entities;
using PCPartsStore.Paging;
using PCPartsStore.Repository.Interfaces;
using PCPartsStore.Services.Interfaces;

namespace PCPartsStore.Controllers;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IProductService _productService;
    private readonly ApplicationDbContext _dbContext;


    public ProductController(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository,
        IProductService productService, ApplicationDbContext dbContext)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _productService = productService;
        _dbContext = dbContext;
    }

    [Route("Product/Index")]
    public IActionResult Index()
    {
        return View();
    }


    [Route("Product/Products/{productCategory}")]
    public IActionResult Products(string productCategory)
    {
        Console.WriteLine("ProductCategory: " + productCategory);

        return View();
    }


    [Route("Product/Categories")]
    public IActionResult Categories(int productId)
    {
        return View();
    }


    [Route("Product/Add")]
    [Authorize(Roles = "Admin")]
    public IActionResult Add()
    {
        var categories = _productCategoryRepository.GetProductCategories();
        ViewData["ProductCategories"] = new SelectList(categories, "Id", "Name");
        return View();
    }

    [Route("/Product/Add/{product}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Add(string product)
    {
        if (product == "Cpu")
        {
            ViewData["AddText"] = "Add a new CPU product";
            var productsCategory = _dbContext.ProductsCategory.Where(p => p.Id == 1).ToList();
            ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
            return View();
        }
        else if (product == "Gpu")
        {
            ViewData["AddText"] = "Add a new GPU product";
            var productsCategory = _dbContext.ProductsCategory.Where(p => p.Id == 2).ToList();
            ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
            return View();
        }

        else if (product == "Ram")
        {
            ViewData["AddText"] = "Add a new RAM product";
            var productsCategory = _dbContext.ProductsCategory.Where(p => p.Id == 3).ToList();
            ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
            return View();
        }

        else if (product == "Motherboard")
        {
            ViewData["AddText"] = "Add a new Motherboard product";
            var productsCategory = _dbContext.ProductsCategory.Where(p => p.Id == 4).ToList();
            ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
            return View();
        }

        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Route("/Product/Add")]
    [Route("Product/Add/{product}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Add(Product model)
    {
        var productsCategory = _dbContext.ProductsCategory.ToList();
        ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
        if (ModelState.IsValid)
        {
            if (_dbContext.Products.Any(id => id.Id == model.Id) == true)
            {
                ModelState.AddModelError("Id",
                    $"Product with id code - \"{model.Id}\" already exists in the database!");
                return View();
            }
            else
            {
                if (await _productService.UploadImage(model.Image, null) == false)
                {
                    ModelState.AddModelError("Image",
                        $"Image with the name - \"{model.Image.FileName}\" already exists in the database!");
                    return View();
                }
                else
                {
                    TempData["ShowProductSuccessfullyAddedToDb"] = true;
                    TempData["ProductId"] = model.Id;
                    await _productService.Add(model);
                    if (model.ProductCategoryId == 1)
                    {
                        return RedirectToAction("Cpu");
                    }
                    else if (model.ProductCategoryId == 2)
                    {
                        return RedirectToAction("Gpu");
                    }
                    else if (model.ProductCategoryId == 3)
                    {
                        return RedirectToAction("Ram");
                    }
                    else if (model.ProductCategoryId == 4)
                    {
                        return RedirectToAction("Motherboard");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
        }

        return View();
    }

    public IActionResult Cpu(int? page)
    {
        var products = _productService.GetProductsByCategory(1);
        var paginatedList = PaginatedList<Product>.Create(products, page ?? 1, 8);
        if (page > paginatedList.TotalPages)
        {
            return NotFound();
        }
        else
        {
            return View(paginatedList);
        }
    }

    [Route("/Product/Cpu/{id}")]
    public IActionResult Cpu(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id && p.ProductCategoryId == 1);
        if (product != null)
        {
            product.ProductImage =
                _dbContext.ProductsImages.Where(i => i.Id == product.ProductImageId).FirstOrDefault();
            return View("Product", product);
        }
        else
        {
            return NotFound();
        }
    }

    public IActionResult Gpu(int? page)
    {
        var products = _productService.GetProductsByCategory(2);
        var paginatedList = PaginatedList<Product>.Create(products, page ?? 1, 8);
        if (page > paginatedList.TotalPages)
        {
            return NotFound();
        }
        else
        {
            return View(paginatedList);
        }
    }

    [Route("/Product/Gpu/{id}")]
    public IActionResult Gpu(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id && p.ProductCategoryId == 2);
        if (product != null)
        {
            product.ProductImage =
                _dbContext.ProductsImages.Where(i => i.Id == product.ProductImageId).FirstOrDefault();
            return View("Product", product);
        }
        else
        {
            return NotFound();
        }
    }

    public IActionResult Ram(int? page)
    {
        var products = _productService.GetProductsByCategory(3);
        var paginatedList = PaginatedList<Product>.Create(products, page ?? 1, 8);
        if (page > paginatedList.TotalPages)
        {
            return NotFound();
        }
        else
        {
            return View(paginatedList);
        }
    }

    [Route("/Product/Ram/{id}")]
    public IActionResult Ram(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id && p.ProductCategoryId == 3);
        if (product != null)
        {
            product.ProductImage =
                _dbContext.ProductsImages.Where(i => i.Id == product.ProductImageId).FirstOrDefault();
            return View("Product", product);
        }
        else
        {
            return NotFound();
        }
    }

    public IActionResult Motherboard(int? page)
    {
        var products = _productService.GetProductsByCategory(4);
        var paginatedList = PaginatedList<Product>.Create(products, page ?? 1, 8);
        if (page > paginatedList.TotalPages)
        {
            return NotFound();
        }
        else
        {
            return View(paginatedList);
        }
    }

    [Route("/Product/Motherboard/{id}")]
    public IActionResult Motherboard(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id && p.ProductCategoryId == 4);
        if (product != null)
        {
            product.ProductImage =
                _dbContext.ProductsImages.Where(i => i.Id == product.ProductImageId).FirstOrDefault();
            return View("Product", product);
        }
        else
        {
            return NotFound();
        }
    }
}