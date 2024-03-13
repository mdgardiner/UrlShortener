using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Context;
using UrlShortener.API.Exceptions;
using UrlShortener.API.Settings;

namespace UrlShortener.API.Configs;

public static class DatabaseConfig
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>()?.Database
            ?? throw new ConfigurationMissingException(nameof(ConnectionStrings));

        services
            .AddDbContext<ShortenedUrlContext>(options => options.UseNpgsql(connectionString))
            .AddScoped<IShortenedUrlContext, ShortenedUrlContext>();
    }
}