using UrlShortener.Infrastructure.DAOs;

namespace UrlShortener.Web.Services.Interfaces;

public interface IUrlService
{
    Task<(UrlDAO? url, string? error)> CreateShortUrl(string originalUrl, Guid userId);
    Task<(bool, string?)> DeleteUrl(Guid urlId, Guid userId, bool isAdmin);
    Task<List<UrlDAO>> GetAllUrls();
    Task<UrlDAO?> GetByShortCode(string shortCode);
}