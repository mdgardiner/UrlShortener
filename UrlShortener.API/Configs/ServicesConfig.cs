using UrlShortener.API.Services;
using UrlShortener.API.Services.Interfaces;

namespace UrlShortener.API.Configs;

public static class ServicesConfig
{
    public static void Configure(IServiceCollection services)
    {
        services
            .AddSingleton<IInputValidator, InputValidator>()
            .AddSingleton<IShortCodeGenerator, ShortCodeGenerator>()
            .AddSingleton<IShortCodeUrlPrefixer, ShortCodeUrlPrefixer>()
            .AddScoped<IShortenedUrlService, ShortenedUrlService>();
    }    
}