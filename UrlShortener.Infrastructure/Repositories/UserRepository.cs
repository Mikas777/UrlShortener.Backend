using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UrlShortener.Infrastructure.DAOs;
using UrlShortener.Infrastructure.Repositories.Interfaces;

namespace UrlShortener.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<UserDAO?> GetByUsername(string username)
    {
        var normalizedUsername = username.ToUpperInvariant();

        logger.LogInformation("Searching for normalized user: {NormalizedUsername}", normalizedUsername);

        return await context.Users
            .Include(u => u.Roles)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NormalizedUsername == normalizedUsername);
    }
}