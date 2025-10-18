using UrlShortener.Domain.Models;
using UrlShortener.Infrastructure.Repositories.Interfaces;
using UrlShortener.Web.Mappers;
using UrlShortener.Web.Services.Interfaces;

namespace UrlShortener.Web.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<UserModel?> ValidateCredentialsAsync(string username, string password)
    {
        username = username?.Trim() ?? string.Empty;

        var user = await userRepository.GetByUsername(username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return null;
        }

        return user.ToModel();
    }
}