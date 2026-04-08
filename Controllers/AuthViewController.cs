using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace ProductApi.Controllers;

public class AuthViewController : Controller
{
    private readonly HttpClient _httpClient;

    public AuthViewController()
    {
        _httpClient = new HttpClient();
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

        var url = $"{Request.Scheme}://{Request.Host}/api/auth/login";

        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Login failed";
            return View();
        }

        var json = await response.Content.ReadAsStringAsync();

        var tokenObj = JsonSerializer.Deserialize<TokenResponse>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        Response.Cookies.Append("jwt", tokenObj.Token);

        return RedirectToAction("Index", "ProductView");
    }

    // GET: Register
public IActionResult Register()
{
    return View();
}

// POST: Register
[HttpPost]
public async Task<IActionResult> Register(string username, string password)
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

    var url = $"{Request.Scheme}://{Request.Host}/api/auth/register";

    var response = await _httpClient.PostAsync(url, content);

    if (!response.IsSuccessStatusCode)
    {
        ViewBag.Error = "Register failed";
        return View();
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