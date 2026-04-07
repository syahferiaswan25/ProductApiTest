using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductController> _logger;
    private readonly IMemoryCache _cache;

    public ProductController(
        IProductRepository repository,
        ILogger<ProductController> logger,
        IMemoryCache cache) 
    {
        _repository = repository;
        _logger = logger;
        _cache = cache;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("Get all products called");

        if (!_cache.TryGetValue("products_all", out IEnumerable<Product> products))
        {
            products = await _repository.GetAll();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(30));

            _cache.Set("products_all", products, cacheOptions);
        }

        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        _logger.LogInformation("Create product called");

        await _repository.Add(product);

        _cache.Remove("products_all");

        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product product)
    {
        _logger.LogInformation("Update product called");

        product.Id = id;
        await _repository.Update(product);

        _cache.Remove("products_all");

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete product called");

        await _repository.Delete(id);

        _cache.Remove("products_all");

        return Ok();
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        string? name,
        decimal? minPrice,
        decimal? maxPrice)
    {
        _logger.LogInformation("Search product called");

        var products = await _repository.GetAll();

        var result = products.Where(p =>
            (string.IsNullOrEmpty(name) || p.Name.Contains(name)) &&
            (!minPrice.HasValue || p.Price >= minPrice) &&
            (!maxPrice.HasValue || p.Price <= maxPrice)
        );

        return Ok(result);
    }

    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new Exception("Ini error test");
    }
}