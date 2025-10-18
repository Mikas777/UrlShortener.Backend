namespace UrlShortener.Infrastructure.DAOs;

public class UserDAO
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public ICollection<RoleDAO> Roles { get; set; } = new List<RoleDAO>();
}