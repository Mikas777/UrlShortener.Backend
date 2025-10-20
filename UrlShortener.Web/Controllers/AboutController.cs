using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Web.Services.Interfaces;
using UrlShortener.Web.ViewModels;

namespace UrlShortener.Web.Controllers
{
    public class AboutController(ISiteContentService siteContentService) : Controller
    {
        [HttpGet("About")]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var viewModel = new AboutViewModel
            {
                Content = await siteContentService.GetAboutTextAsync(),
                IsAdmin = User.Identity?.IsAuthenticated == true && User.IsInRole("Admin")
            };

            return View(viewModel);
        }

        [HttpPost("About")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AboutViewModel model)
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                model.IsAdmin = true;
                return View(model);
            }

            await siteContentService.SetAboutTextAsync(model.Content);

            return RedirectToAction("Index");
        }
    }
}
