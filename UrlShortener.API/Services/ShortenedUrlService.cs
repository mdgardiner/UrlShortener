using UrlShortener.API.Entities;
using UrlShortener.API.Repositories.Interfaces;
using UrlShortener.API.Services.Interfaces;

namespace UrlShortener.API.Services;

public class ShortenedUrlService : IShortenedUrlService
{
    private readonly IUrlValidator _urlValidator;
    private readonly IRepository<ShortenedUrl> _shortenedUrlRepository;
    private readonly IShortCodeUrlPrefixer _shortCodeUrlPrefixer;
    private readonly IShortCodeGenerator _shortCodeGenerator;
    private readonly ILogger<ShortenedUrlService> _logger;

    public ShortenedUrlService(
        IUrlValidator urlValidator,
        IRepository<ShortenedUrl> shortenedUrlRepository,
        IShortCodeUrlPrefixer shortCodeUrlPrefixer,
        IShortCodeGenerator shortCodeGenerator,
        ILogger<ShortenedUrlService> logger)
    {
        _urlValidator = urlValidator;
        _shortenedUrlRepository = shortenedUrlRepository;
        _shortCodeUrlPrefixer = shortCodeUrlPrefixer;
        _shortCodeGenerator = shortCodeGenerator;
        _logger = logger;
    }

    public Uri GetShortenedUrl(string? longUrl)
    {
        if (!_urlValidator.IsValid(longUrl))
            throw new ArgumentException($"{longUrl} is not a valid url");

        var existingShortUrl = _shortenedUrlRepository
            .SearchForSingleOrDefault(x => x.LongUrl == longUrl);

        if (existingShortUrl != null)
            return _shortCodeUrlPrefixer.GetPrefixedUrl(existingShortUrl.ShortCode);

        var newShortCode = _shortCodeGenerator.GenerateShortCode(7);

        var shortenedUrl = new ShortenedUrl
        {
            LongUrl = longUrl!,
            ShortCode = newShortCode
        };

        _shortenedUrlRepository.Insert(shortenedUrl);
        _shortenedUrlRepository.Commit();
        
        return _shortCodeUrlPrefixer.GetPrefixedUrl(newShortCode);
    }

    public Uri RetrieveLongUrl(string? shortUrl)
    {
        throw new NotImplementedException();
    }
}