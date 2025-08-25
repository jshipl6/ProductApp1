using Microsoft.AspNetCore.Mvc;
using ProductApp.Models;

namespace ProductApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        // quick in-memory list so we can "add" without a database
        private static readonly List<Product> _products =
        [
            new Product { Id = 1, Name = "Guitar Picks", Price = 4.99m },
            new Product { Id = 2, Name = "Patch",       Price = 6.50m },
            new Product { Id = 3, Name = "Sticker",     Price = 1.99m }
        ];

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        // GET: /Product
        public IActionResult Index()
        {
            _logger.LogInformation("Loaded Product/Index with {Count} items.", _products.Count);
            return View(_products);
        }

        // POST: /Product/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Product newProduct)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid product submitted: {@Product}", newProduct);
                return View("Index", _products);
            }

            newProduct.Id = _products.Count == 0 ? 1 : _products.Max(p => p.Id) + 1;
            _products.Add(newProduct);

            _logger.LogInformation("Added product {Name} at ${Price}", newProduct.Name, newProduct.Price);
            return RedirectToAction(nameof(Index));
        }
    }
}
