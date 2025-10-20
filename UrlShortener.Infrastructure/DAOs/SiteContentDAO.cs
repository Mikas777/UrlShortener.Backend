using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Infrastructure.DAOs;

public class SiteContentDAO
{
    [Key]
    [MaxLength(200)]
    public required string Key { get; set; }
    public required string Value { get; set; }
}