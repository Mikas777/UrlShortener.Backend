namespace UrlShortener.Infrastructure.DAOs;

public class UserRoleDAO
{
    public required Guid UserId { get; set; }
    public UserDAO User { get; set; } = null!;
    public required Guid RoleId { get; set; }
    public RoleDAO Role { get; set; } = null!;
}