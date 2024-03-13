using UrlShortener.API.Entities;
using UrlShortener.API.Repositories;
using UrlShortener.API.Repositories.Interfaces;

namespace UrlShortener.API.Configs;

public static class RepositoryConfig
{
    public static void Configure(IServiceCollection services)
    {
        services.AddScoped<IRepository<ShortenedUrl>, ShortenedUrlRepository>();
    }
}