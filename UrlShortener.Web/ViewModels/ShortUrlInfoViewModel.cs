namespace UrlShortener.Web.ViewModels;

public class ShortUrlInfoViewModel
{
    public Guid Id { get; set; }
    public required string OriginalUrl { get; set; }
    public required string ShortCode { get; set; }
    public DateTime CreatedDate { get; set; }
    public required string CreatedByUsername { get; set; }
}