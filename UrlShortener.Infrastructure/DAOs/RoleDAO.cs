namespace UrlShortener.Infrastructure.DAOs;

public class RoleDAO
{
    public Guid Id { get; set; }
    public required string Name { get; set; } = null!;
    public ICollection<UserDAO> Users { get; set; } = new List<UserDAO>();
}