namespace UrlShortener.Web.DTOs;

public class UrlItemResponse
{
    public Guid Id { get; set; }
    public required string OriginalUrl { get; set; }
    public required string ShortCode { get; set; }
    public Guid CreatedById { get; set; }
}