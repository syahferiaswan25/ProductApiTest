using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using ProductApi.Models;

namespace ProductApi.Controllers;

public class AuthViewController : Controller
{
    private readonly HttpClient _httpClient;

    public AuthViewController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var request = new
        {
            Username = username,
            Password = password
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json"
        );

        //BaseAddress + relative URL
        var scheme = Request.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? Request.Scheme;
_httpClient.BaseAddress = new Uri($"{scheme}://{Request.Host}");
        var response = await _httpClient.PostAsync("/api/auth/login", content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            ViewBag.Error = $"Login failed: {error}";
            return View();
        }

        var json = await response.Content.ReadAsStringAsync();

        var tokenObj = JsonSerializer.Deserialize<TokenResponse>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        //HTTPS (Railway)
        Response.Cookies.Append("jwt", tokenObj.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });

        return RedirectToAction("Index", "ProductView");
    }

    // GET: Register
    public IActionResult Register()
    {
    return View(new RegisterRequest());
    }

    // POST: Register
    [HttpPost]
public async Task<IActionResult> Register(RegisterRequest request){
    if (!ModelState.IsValid)
        return View(request);

    var content = new StringContent(
        JsonSerializer.Serialize(request),
        Encoding.UTF8,
        "application/json"
    );

    var scheme = Request.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? Request.Scheme;
_httpClient.BaseAddress = new Uri($"{scheme}://{Request.Host}");

    var response = await _httpClient.PostAsync("/api/auth/register", content);

    if (!response.IsSuccessStatusCode)
    {
        var error = await response.Content.ReadAsStringAsync();
        ViewBag.Error = $"Register failed: {error}";
        return View(request);
    }

    return RedirectToAction("Login");
}

    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        return RedirectToAction("Login");
    }
}

public class TokenResponse
{
    public string Token { get; set; }
}