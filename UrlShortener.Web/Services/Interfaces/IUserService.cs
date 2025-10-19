using UrlShortener.Domain.Models;

namespace UrlShortener.Web.Services.Interfaces;

public interface IUserService
{
    Task<UserModel?> ValidateCredentials(string username, string password);
}