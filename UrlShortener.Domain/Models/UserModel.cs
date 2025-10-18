namespace UrlShortener.Domain.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public List<RoleModel> Roles { get; set; } = [];
}