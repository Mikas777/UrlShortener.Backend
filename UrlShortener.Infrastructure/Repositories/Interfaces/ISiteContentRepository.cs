using UrlShortener.Infrastructure.DAOs;

namespace UrlShortener.Infrastructure.Repositories.Interfaces;

public interface ISiteContentRepository
{
    Task<SiteContentDAO?> GetByKey(string key);
    Task UpsertContent(SiteContentDAO content);
}