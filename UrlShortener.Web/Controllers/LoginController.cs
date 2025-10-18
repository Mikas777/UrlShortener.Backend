using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrlShortener.Web.Services.Interfaces;
using UrlShortener.Web.ViewModels;

namespace UrlShortener.Web.Controllers;

public class LoginController(
    IUserService userService,
    ILogger<LoginController> logger,
    IConfiguration configuration)
    : Controller
{
    private readonly string _clientOrigin = configuration.GetValue<string>("ClientOrigin")!;

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Index(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var user = await userService.ValidateCredentialsAsync(model.Username, model.Password);

            if (user == null)
            {
                logger.LogWarning("Failed login attempt for username: {Username}", model.Username);
                ModelState.AddModelError(string.Empty, "Invalid username or password.");

                return View(model);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
            };
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            logger.LogInformation("UserModel {Username} logged in successfully.", user.Username);
            return RedirectToLocal(returnUrl);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during login attempt for {Username}", model.Username);
            ModelState.AddModelError(string.Empty, "An internal server error occurred.");

            return View(model);
        }
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
      
        if (!string.IsNullOrEmpty(returnUrl) && returnUrl.StartsWith(_clientOrigin))
        {
            return Redirect(returnUrl);
        }

        return Redirect(_clientOrigin);
    }
}