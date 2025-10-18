using Microsoft.EntityFrameworkCore;
using UrlShortener.Infrastructure;
using UrlShortener.Infrastructure.Repositories;
using UrlShortener.Infrastructure.Repositories.Interfaces;
using UrlShortener.Web.Services;
using UrlShortener.Web.Services.Interfaces;

namespace UrlShortener.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnection = configuration.GetConnectionString("DefaultConnection");

        services.AddDatabaseContext(defaultConnection);
        services.AddRepositories();

        return services;
    }

    public static void AddDatabaseContext(this IServiceCollection serviceCollection, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Connection string is empty or null.", nameof(connectionString));
        }

        serviceCollection.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }
}