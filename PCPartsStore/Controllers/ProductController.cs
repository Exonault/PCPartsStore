using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PCPartsStore.Entities;
using PCPartsStore.Repository.Interfaces;
using PCPartsStore.Services.Interfaces;

namespace PCPartsStore.Controllers;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IProductService _productService;


    public ProductController(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository,
        IProductService productService)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _productService = productService;
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
        if (product == null)
        {
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
        var categories = _productCategoryRepository.GetProductCategories();
        ViewData["ProductCategories"] = new SelectList(categories, "Id", "Name");

        if (ModelState.IsValid)
        {
            if (_productRepository.ContainsProductWithId(model.Id))
            {
                ModelState.AddModelError("Id",
                    $"Product with id code - \"{model.Id}\" already exists in the database!");
                return View();
            }

            else
            {
                if (await _productService.UploadImage(model.Image, null))
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
}