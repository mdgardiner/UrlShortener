using Microsoft.Extensions.Options;
using UrlShortener.API.Entities;
using UrlShortener.API.Repositories.Interfaces;
using UrlShortener.API.Services.Interfaces;
using UrlShortener.API.Settings;

namespace UrlShortener.API.Services;

public class ShortenedUrlService : IShortenedUrlService
{
    private readonly IUrlValidator _urlValidator;
    private readonly IRepository<ShortenedUrl> _shortenedUrlRepository;
    private readonly IShortCodeUrlPrefixer _shortCodeUrlPrefixer;
    private readonly IShortCodeGenerator _shortCodeGenerator;
    private readonly ApplicationSettings _applicationSettings;
    private readonly ILogger<ShortenedUrlService> _logger;

    public ShortenedUrlService(
        IUrlValidator urlValidator,
        IRepository<ShortenedUrl> shortenedUrlRepository,
        IShortCodeUrlPrefixer shortCodeUrlPrefixer,
        IShortCodeGenerator shortCodeGenerator,
        IOptions<ApplicationSettings> applicationSettings,
        ILogger<ShortenedUrlService> logger)
    {
        _urlValidator = urlValidator;
        _shortenedUrlRepository = shortenedUrlRepository;
        _shortCodeUrlPrefixer = shortCodeUrlPrefixer;
        _shortCodeGenerator = shortCodeGenerator;
        _applicationSettings = applicationSettings.Value;
        _logger = logger;
    }

    public Uri GetShortenedUrl(string? longUrl)
    {
        _logger.LogInformation("Getting shortened url for '{longUrl}'", longUrl);
        
        if (!_urlValidator.IsValid(longUrl))
            throw new ArgumentException($"{longUrl} is not a valid url");

        var existingShortUrl = _shortenedUrlRepository
            .SearchForSingleOrDefault(x => x.LongUrl == longUrl);

        if (existingShortUrl != null)
            return GetAndLogReturnUrl(longUrl, existingShortUrl.ShortCode);

        var newShortCode = CreateNewShortCode(longUrl);

        return GetAndLogReturnUrl(longUrl, newShortCode);
    }

    public Uri RetrieveLongUrl(string? shortUrl)
    {
        throw new NotImplementedException();
    }
    
    private string CreateNewShortCode(string? longUrl)
    {
        var isUniqueShortCode = false;
        var newShortCode = string.Empty;

        while (!isUniqueShortCode)
        {
            newShortCode = _shortCodeGenerator.GenerateShortCode(_applicationSettings.ShortCodeLength);

            var existingShortUrl = _shortenedUrlRepository
                .SearchForSingleOrDefault(x => x.ShortCode == newShortCode);

            if (existingShortUrl == null)
                isUniqueShortCode = true;
        }
        
        var shortenedUrl = new ShortenedUrl
        {
            LongUrl = longUrl!,
            ShortCode = newShortCode
        };

        _shortenedUrlRepository.Insert(shortenedUrl);
        _shortenedUrlRepository.Commit();
        return newShortCode;
    }

    private Uri GetAndLogReturnUrl(string? longUrl, string shortCode)
    {
        var shortenedUrl = _shortCodeUrlPrefixer.GetPrefixedUrl(shortCode);
        _logger.LogInformation("Returning {shortenedUrl} for {longUrl}", shortenedUrl, longUrl);
        return shortenedUrl;
    }
}