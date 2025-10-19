using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Web.DTOs;

public class CreateUrlRequest
{
    [Required(ErrorMessage = "OriginalUrl is required.")]
    [Url(ErrorMessage = "A valid URL is required.")]
    public required string OriginalUrl { get; set; }
}