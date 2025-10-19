using Microsoft.EntityFrameworkCore;
using UrlShortener.Infrastructure.DAOs;
using UrlShortener.Infrastructure.Repositories.Interfaces;

namespace UrlShortener.Infrastructure.Repositories;

public class UrlRepository(ApplicationDbContext context) : IUrlRepository
{
    public async Task<UrlDAO?> GetByOriginalUrl(string originalUrl)
    {
        return await context.Urls
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.OriginalUrl == originalUrl);
    }

    public async Task<bool> ShortCodeExists(string shortCode)
    {
        return await context.Urls.AnyAsync(u => u.ShortCode == shortCode);
    }

    public async Task<List<UrlDAO>> GetAllUrls()
    {
        return await context.Urls
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<UrlDAO> AddUrl(UrlDAO newUrl)
    {
        context.Urls.Add(newUrl);
        await context.SaveChangesAsync();

        return newUrl;
    }

    public async Task<UrlDAO?> GetById(Guid urlId)
    {
        return await context.Urls.FindAsync(urlId);
    }

    public async Task DeleteUrl(UrlDAO url)
    {
        context.Urls.Remove(url);
        await context.SaveChangesAsync();
    }
    public async Task<UrlDAO?> GetByShortCode(string shortCode)
    {
        return await context.Urls
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.ShortCode == shortCode);
    }
}