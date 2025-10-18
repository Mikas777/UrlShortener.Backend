using UrlShortener.Infrastructure.DAOs;

namespace UrlShortener.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    Task<UserDAO?> GetByUsername(string username);
}