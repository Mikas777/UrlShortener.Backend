using UrlShortener.Domain.Models;
using UrlShortener.Infrastructure.DAOs;

namespace UrlShortener.Web.Services.Interfaces;

public interface IUserService
{
    Task<UserModel?> ValidateCredentials(string username, string password);
}