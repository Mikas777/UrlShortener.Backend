using UrlShortener.Infrastructure.DAOs;
using UrlShortener.Web.DTOs;

namespace UrlShortener.Web.Mappers;

public static class UrlMapper
{
    public static UrlItemResponse ToUrlItemResponse(this UrlDAO dao)
    {
        return new UrlItemResponse
        {
            Id = dao.Id,
            OriginalUrl = dao.OriginalUrl,
            ShortCode = dao.ShortCode,
            CreatedById = dao.CreatedById
        };
    }
}