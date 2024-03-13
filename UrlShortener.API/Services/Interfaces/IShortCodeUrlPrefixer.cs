namespace UrlShortener.API.Services.Interfaces;

public interface IShortCodeUrlPrefixer
{
    Uri GetPrefixedUrl(string shortcode);
}