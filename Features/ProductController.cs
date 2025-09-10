using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApp.Data;
using ProductApp.Models;
using ProductApp.Services;

namespace ProductApp.Features.Products
{
    [Route("products")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IPriceCalculator _priceCalc;
        private readonly ApplicationDbContext _db;
        private const decimal TaxRate = 0.07m;

        public ProductController(
            ILogger<ProductController> logger,
            IPriceCalculator priceCalc,
            ApplicationDbContext db)
        {
            _logger = logger;
            _priceCalc = priceCalc;
            _db = db;
        }

        // GET /products?q=term
        [HttpGet("")]
        public async Task<IActionResult> Index(string? q)
        {
            IQueryable<Product> query = _db.Products.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(term));
            }

            var list = await query.OrderBy(p => p.Id).ToListAsync();

            foreach (var p in list)
                p.PriceWithTax = _priceCalc.AddTax(p.Price, TaxRate);

            ViewBag.Search = q ?? string.Empty;
            _logger.LogInformation("Products Index: {Count} items, filter '{Filter}'", list.Count, q);
            return View(list);
        }

        // GET /products/create
        [HttpGet("create")]
        public IActionResult Create() => View(new Product());

        // POST /products/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model)
        {
            if (!ModelState.IsValid) return View(model);

            _db.Products.Add(model);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Created product {Id}:{Name}", model.Id, model.Name);
            return RedirectToAction(nameof(Index));
        }

        // GET /products/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return NotFound();

            product.PriceWithTax = _priceCalc.AddTax(product.Price, TaxRate);
            return View(product);
        }

        // GET /products/{id}/edit
        [HttpGet("{id:int}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product is null) return NotFound();
            return View(product);
        }

        // POST /products/{id}/edit
        [HttpPost("{id:int}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            _logger.LogInformation("Edited product {Id}:{Name}", model.Id, model.Name);
            return RedirectToAction(nameof(Index));
        }

        // GET /products/{id}/delete
        [HttpGet("{id:int}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return NotFound();

            product.PriceWithTax = _priceCalc.AddTax(product.Price, TaxRate);
            return View(product);
        }

        // POST /products/{id}/delete
        [HttpPost("{id:int}/delete"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product is null) return NotFound();

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Deleted product {Id}:{Name}", product.Id, product.Name);
            return RedirectToAction(nameof(Index));
        }
    }
}

