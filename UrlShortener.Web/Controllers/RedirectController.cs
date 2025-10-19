using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Web.Services.Interfaces;

namespace UrlShortener.Web.Controllers;

[AllowAnonymous]
public class RedirectController(IUrlService urlService,ILogger<RedirectController> logger) : Controller
{
    [HttpGet("{shortCode:length(7)}")]
    public async Task<IActionResult> Index(string shortCode)
    {
        logger.LogInformation("Redirect request received for ShortCode: {ShortCode}", shortCode);

        var url = await urlService.GetByShortCode(shortCode);

        if (url == null)
        {
            logger.LogWarning("ShortCode {ShortCode} not found.", shortCode);
            return NotFound();
        }
        
        return RedirectPermanent(url.OriginalUrl);
    }
}