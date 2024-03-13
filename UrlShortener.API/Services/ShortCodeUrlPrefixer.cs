using Microsoft.Extensions.Options;
using UrlShortener.API.Services.Interfaces;
using UrlShortener.API.Settings;

namespace UrlShortener.API.Services;

public class ShortCodeUrlPrefixer : IShortCodeUrlPrefixer
{
    private readonly ApplicationSettings _applicationSettings;

    public ShortCodeUrlPrefixer(IOptions<ApplicationSettings> applicationSettings)
    {
        _applicationSettings = applicationSettings.Value;
    }

    public Uri GetPrefixedUrl(string shortcode) =>
        new(string.Join('/', _applicationSettings.ShortCodeUrlPrefix, shortcode));
}