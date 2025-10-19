using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Infrastructure.Repositories.Interfaces;
using UrlShortener.Web.DTOs;
using UrlShortener.Web.Mappers;
using UrlShortener.Web.Services.Interfaces;

namespace UrlShortener.Web.Controllers.Api;

[ApiController]
[Route("api/v1/[controller]")]
public class UrlsController(IUrlService urlService, ILogger<UrlsController> logger) : ControllerBase
{
    private bool IsCurrentUserAdmin => User.IsInRole("Admin");

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetUrls()
    {
        logger.LogInformation("Fetching all URLs for public table view");

        var urlDaos = await urlService.GetAllUrls();
        var urlResponses = urlDaos.Select(dao => dao.ToUrlItemResponse());

        return Ok(urlResponses);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateShortUrl([FromBody] CreateUrlRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (url, error) = await urlService.CreateShortUrl(request.OriginalUrl, userId.Value);

        if (error != null)
        {
            return BadRequest(new { Message = error });
        }

        return CreatedAtAction(nameof(GetUrls), new { id = url!.Id }, url.ToUrlItemResponse());
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteUrl(Guid id)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var (success, error) = await urlService.DeleteUrl(id, userId.Value, IsCurrentUserAdmin);

        if (!success)
        {
            return BadRequest(new { Message = error });
        }

        return NoContent();
    }

    private Guid? GetCurrentUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return userIdString != null ? Guid.Parse(userIdString) : null;
    }
}