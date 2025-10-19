namespace UrlShortener.Infrastructure.DAOs;

public class UrlDAO
{
    public Guid Id { get; set; }
    public required string OriginalUrl { get; set; }
    public required string ShortCode { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedById { get; set; }
    public UserDAO CreatedBy { get; set; } = null!;
}