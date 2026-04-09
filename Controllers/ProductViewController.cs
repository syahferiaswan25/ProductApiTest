using Microsoft.AspNetCore.Mvc;
using ProductApi.Data;
using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

namespace ProductApi.Controllers;

public class ProductViewController : Controller
{
    private readonly AppDbContext _context;

    public ProductViewController(AppDbContext context)
    {
        _context = context;
    }

    private bool IsLoggedIn()
    {
        return Request.Cookies["jwt"] != null;
    }

    // LIST + SEARCH + FILTER (FINAL)
    public async Task<IActionResult> Index(string search, decimal? minPrice, decimal? maxPrice)
    {
        if (!IsLoggedIn())
            return RedirectToAction("Login", "AuthView");

        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Name != null && p.Name.Contains(search));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        var products = await query.ToListAsync();

        return View(products);
    }

    // GET: Create
    public IActionResult Create()
    {
        if (!IsLoggedIn())
            return RedirectToAction("Login", "AuthView");

        return View();
    }

    // POST: Create
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        if (!IsLoggedIn())
            return RedirectToAction("Login", "AuthView");

        if (ModelState.IsValid)
        {
            product.CreatedAt = DateTime.UtcNow;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    // GET: Edit
    public async Task<IActionResult> Edit(int id)
    {
        if (!IsLoggedIn())
            return RedirectToAction("Login", "AuthView");

        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        return View(product);
    }

    // POST: Edit
    [HttpPost]
    public async Task<IActionResult> Edit(Product product)
    {
        if (!IsLoggedIn())
            return RedirectToAction("Login", "AuthView");

        if (ModelState.IsValid)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    // DELETE
    public async Task<IActionResult> Delete(int id)
    {
        if (!IsLoggedIn())
            return RedirectToAction("Login", "AuthView");

        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}