using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Web.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Login is required")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}