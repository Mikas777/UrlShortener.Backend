using UrlShortener.Infrastructure.DAOs;

namespace UrlShortener.Infrastructure.Repositories.Interfaces;

public interface IUrlRepository
{
    Task<UrlDAO?> GetByOriginalUrl(string originalUrl);
    Task<bool> ShortCodeExists(string shortCode);
    Task<UrlDAO> AddUrl(UrlDAO newUrl);
    Task<UrlDAO?> GetById(Guid urlId);
    Task DeleteUrl(UrlDAO url);
    Task<List<UrlDAO>> GetAllUrls();
    Task<UrlDAO?> GetByShortCode(string shortCode);
}