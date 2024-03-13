namespace UrlShortener.API.Services.Interfaces;

public interface IShortenedUrlService
{
    Uri GetShortenedUrl(string? longUrl);
    Uri RetrieveLongUrl(string? shortUrl);
}