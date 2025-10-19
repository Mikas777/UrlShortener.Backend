using UrlShortener.Domain.Models;
using UrlShortener.Infrastructure.Repositories.Interfaces;
using UrlShortener.Web.Mappers;
using UrlShortener.Web.Services.Interfaces;

namespace UrlShortener.Web.Services;

public class UserService(IUserRepository userRepository,
    IUrlRepository urlRepository,
    ILogger<UserService> logger) : IUserService
{
    public async Task<UserModel?> ValidateCredentials(string username, string password)
    {
        username = username?.Trim() ?? string.Empty;

        var user = await userRepository.GetByUsername(username);

        // Temporarily disable password verification, because passwords are stored in plain text.
        //if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        //{
        //    return null;
        //}

        if (user == null || password != user.PasswordHash)
        {
            return null;
        }

        return user.ToModel();
    }
}