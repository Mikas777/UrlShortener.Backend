using UrlShortener.Domain.Models;
using UrlShortener.Infrastructure.DAOs;

namespace UrlShortener.Web.Mappers;

public static class RoleMapper
{
    public static RoleModel ToModel(this RoleDAO roleDao)
    {
        return new RoleModel
        {
            Id = roleDao.Id,
            Name = roleDao.Name
        };
    }
}