using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Web.ViewModels;

public class AboutViewModel
{
    [Required]
    public string Content { get; set; } = string.Empty;
    public bool IsAdmin { get; set; } = false;
}