using Microsoft.AspNetCore.Mvc;
using ProductApp.Models;
using ProductApp.Services;

namespace ProductApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IPriceCalculator _priceCalc;

       
        private static readonly List<Product> _products =
        [
            new Product { Id = 1, Name = "Guitar Picks", Price = 4.99m },
            new Product { Id = 2, Name = "Patch",       Price = 6.50m },
            new Product { Id = 3, Name = "Sticker",     Price = 1.99m }
        ];

        // pick tax rate
        private const decimal TaxRate = 0.07m;

        public ProductController(ILogger<ProductController> logger, IPriceCalculator priceCalc)
        {
            _logger = logger;
            _priceCalc = priceCalc;
        }

        public IActionResult Index()
        {
            foreach (var p in _products)
                p.PriceWithTax = _priceCalc.AddTax(p.Price, TaxRate);

            _logger.LogInformation("Loaded Product/Index with {Count} items.", _products.Count);
            return View(_products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Product newProduct)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid product submitted: {@Product}", newProduct);
                foreach (var p in _products)
                    p.PriceWithTax = _priceCalc.AddTax(p.Price, TaxRate);
                return View("Index", _products);
            }

            newProduct.Id = _products.Count == 0 ? 1 : _products.Max(p => p.Id) + 1;
            newProduct.PriceWithTax = _priceCalc.AddTax(newProduct.Price, TaxRate);
            _products.Add(newProduct);

            _logger.LogInformation("Added product {Name} at ${Price}", newProduct.Name, newProduct.Price);
            return RedirectToAction(nameof(Index));
        }
    }
}
