using UrlShortener.Infrastructure.DAOs;
using UrlShortener.Infrastructure.Repositories.Interfaces;
using UrlShortener.Web.Services.Interfaces;

namespace UrlShortener.Web.Services;

public class SiteContentService(ISiteContentRepository repository) : ISiteContentService
{
    private const string AboutContentKey = "AboutPageContent";

    public async Task<string> GetAboutTextAsync()
    {
        var content = await repository.GetByKey(AboutContentKey);

        return content?.Value ?? "Default About Page text. Please edit.";
    }

    public async Task SetAboutTextAsync(string contentValue)
    {
        var contentDao = new SiteContentDAO
        {
            Key = AboutContentKey,
            Value = contentValue
        };

        await repository.UpsertContent(contentDao);
    }
}