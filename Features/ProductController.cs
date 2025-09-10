using Microsoft.AspNetCore.Mvc;
using ProductApp.Models;
using ProductApp.Services;

namespace ProductApp.Features.Products
{
    // Attribute routed controller #2
    // Base path for all product URLs
    [Route("products")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IPriceCalculator _priceCalc;

        // In-memory store for the assignment
        private static readonly List<Product> _products =
        [
            new Product { Id = 1, Name = "Guitar Picks", Price = 4.99m },
            new Product { Id = 2, Name = "Patch",       Price = 6.50m },
            new Product { Id = 3, Name = "Sticker",     Price = 1.99m }
        ];

        private const decimal TaxRate = 0.07m;

        public ProductController(ILogger<ProductController> logger, IPriceCalculator priceCalc)
        {
            _logger = logger;
            _priceCalc = priceCalc;
        }

        // GET /products?q=term
        [HttpGet("")]
        public IActionResult Index(string? q)
        {
            IEnumerable<Product> query = _products;

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim().ToLowerInvariant();
                query = query.Where(p => p.Name.ToLowerInvariant().Contains(term));
            }

            foreach (var p in query)
                p.PriceWithTax = _priceCalc.AddTax(p.Price, TaxRate);

            ViewBag.Search = q ?? string.Empty;
            _logger.LogInformation("Products Index: {Count} items, filter '{Filter}'", query.Count(), q);
            return View(query.ToList());
        }

        // GET /products/create
        [HttpGet("create")]
        public IActionResult Create() => View(new Product());

        // POST /products/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model)
        {
            if (!ModelState.IsValid) return View(model);

            model.Id = _products.Count == 0 ? 1 : _products.Max(p => p.Id) + 1;
            model.PriceWithTax = _priceCalc.AddTax(model.Price, TaxRate);
            _products.Add(model);

            _logger.LogInformation("Created product {Id}:{Name}", model.Id, model.Name);
            return RedirectToAction(nameof(Index));
        }

        // GET /products/{id}
        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product is null) return NotFound();

            product.PriceWithTax = _priceCalc.AddTax(product.Price, TaxRate);
            return View(product);
        }

        // GET /products/{id}/edit
        [HttpGet("{id:int}/edit")]
        public IActionResult Edit(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product is null) return NotFound();

            return View(new Product { Id = product.Id, Name = product.Name, Price = product.Price });
        }

        // POST /products/{id}/edit
        [HttpPost("{id:int}/edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var existing = _products.FirstOrDefault(p => p.Id == id);
            if (existing is null) return NotFound();

            existing.Name = model.Name;
            existing.Price = model.Price;
            existing.PriceWithTax = _priceCalc.AddTax(existing.Price, TaxRate);

            _logger.LogInformation("Edited product {Id}:{Name}", existing.Id, existing.Name);
            return RedirectToAction(nameof(Index));
        }

        // GET /products/{id}/delete
        [HttpGet("{id:int}/delete")]
        public IActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product is null) return NotFound();

            product.PriceWithTax = _priceCalc.AddTax(product.Price, TaxRate);
            return View(product);
        }

        // POST /products/{id}/delete
        [HttpPost("{id:int}/delete"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product is null) return NotFound();

            _products.Remove(product);
            _logger.LogInformation("Deleted product {Id}:{Name}", product.Id, product.Name);
            return RedirectToAction(nameof(Index));
        }
    }
}
