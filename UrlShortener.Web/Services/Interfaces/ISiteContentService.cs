namespace UrlShortener.Web.Services.Interfaces;

public interface ISiteContentService
{
    Task<string> GetAboutTextAsync();
    Task SetAboutTextAsync(string content);
}