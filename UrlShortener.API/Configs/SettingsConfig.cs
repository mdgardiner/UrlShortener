using UrlShortener.API.Settings;

namespace UrlShortener.API.Configs;

public static class SettingsConfig
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        var applicationSettings = configuration.GetSection(nameof(ApplicationSettings));

        services
            .Configure<ApplicationSettings>(applicationSettings);
    }
    
}