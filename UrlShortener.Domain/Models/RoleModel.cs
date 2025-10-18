namespace UrlShortener.Domain.Models;

public class RoleModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}