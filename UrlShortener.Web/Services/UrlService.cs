using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UrlShortener.Infrastructure;
using UrlShortener.Infrastructure.DAOs;
using UrlShortener.Infrastructure.Repositories;
using UrlShortener.Infrastructure.Repositories.Interfaces;
using UrlShortener.Web.Services.Interfaces;

namespace UrlShortener.Web.Services;

public class UrlService(IUrlRepository urlRepository, ILogger<UrlService> logger) : IUrlService
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int ShortCodeLength = 7;
    public async Task<List<UrlDAO>> GetAllUrls()
    {
        return await urlRepository.GetAllUrls();
    }

    public async Task<UrlDAO?> GetByShortCode(string shortCode)
    {
        logger.LogInformation("Service searching for ShortCode: {ShortCode}", shortCode);

        var url = await urlRepository.GetByShortCode(shortCode);

        if (url == null)
        {
            logger.LogWarning("Service: ShortCode {ShortCode} not found in repository.", shortCode);
        }

        return url;
    }

    public async Task<(UrlDAO? url, string? error)> CreateShortUrl(string originalUrl, Guid userId)
    {
        var existing = await urlRepository.GetByOriginalUrl(originalUrl);

        if (existing != null)
        {
            logger.LogInformation("URL already exists. Returning existing ShortCode: {ShortCode}", existing.ShortCode);
            return (null, "This URL has already been shortened.");
        }

        string shortCode;
        do
        {
            shortCode = GenerateRandomShortCode();
        }
        while (await urlRepository.ShortCodeExists(shortCode));

        logger.LogInformation("Generated unique ShortCode: {ShortCode}", shortCode);

        var newUrl = new UrlDAO
        {
            Id = Guid.NewGuid(),
            OriginalUrl = originalUrl,
            ShortCode = shortCode,
            CreatedDate = DateTime.UtcNow,
            CreatedById = userId
        };

        await urlRepository.AddUrl(newUrl);

        return (newUrl, null);
    }

    public async Task<(bool, string?)> DeleteUrl(Guid urlId, Guid userId, bool isAdmin)
    {
        var url = await urlRepository.GetById(urlId);

        if (url == null)
        {
            return (false, "URL not found.");
        }

        if (url.CreatedById != userId && !isAdmin)
        {
            logger.LogWarning("FORBIDDEN: User {UserId} tried to delete URL {UrlId} owned by {OwnerId}", userId, urlId, url.CreatedById);

            return (false, "You do not have permission to delete this URL.");
        }

        await urlRepository.DeleteUrl(url);

        logger.LogInformation("URL {UrlId} deleted by User {UserId} (IsAdmin: {IsAdmin})", urlId, userId, isAdmin);

        return (true, null);
    }

    private static string GenerateRandomShortCode()
    {
        var sb = new StringBuilder(ShortCodeLength);
        var buffer = new byte[ShortCodeLength];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(buffer);
        }

        for (var i = 0; i < ShortCodeLength; i++)
        {
            var index = buffer[i] % Alphabet.Length;
            sb.Append(Alphabet[index]);
        }

        return sb.ToString();
    }
}