using Microsoft.EntityFrameworkCore;
using UrlShortener.Infrastructure.DAOs;
using UrlShortener.Infrastructure.Repositories.Interfaces;

namespace UrlShortener.Infrastructure.Repositories;

public class SiteContentRepository(ApplicationDbContext context) : ISiteContentRepository
{
    public async Task<SiteContentDAO?> GetByKey(string key)
    {
        return await context.SiteContents
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Key == key);
    }

    public async Task UpsertContent(SiteContentDAO content)
    {
        var existing = await context.SiteContents.FirstOrDefaultAsync(c => c.Key == content.Key);

        if (existing != null)
        {
            existing.Value = content.Value;
            context.SiteContents.Update(existing);
        }
        else
        {
            context.SiteContents.Add(content);
        }

        await context.SaveChangesAsync();
    }
}