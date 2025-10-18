using UrlShortener.Domain.Models;
using UrlShortener.Infrastructure.DAOs;

namespace UrlShortener.Web.Mappers;

public static class UserMapper
{
    public static UserModel ToModel(this UserDAO userDao)
    {
        return new UserModel
        {
            Id = userDao.Id,
            Username = userDao.Username,
            PasswordHash = userDao.PasswordHash,
            Roles = userDao.Roles.Select(r => r.ToModel()).ToList()
        };
    }
}