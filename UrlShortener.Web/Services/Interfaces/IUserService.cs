using UrlShortener.Domain.Models;

namespace UrlShortener.Web.Services.Interfaces;

public interface IUserService
{
    Task<UserModel?> ValidateCredentialsAsync(string username, string password);
}