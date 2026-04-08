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

    // List Product
    public async Task<IActionResult> Index()
    {
        if (!IsLoggedIn())
            return RedirectToAction("Login", "AuthView");

        var products = await _context.Products.ToListAsync();
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

    // Delete
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