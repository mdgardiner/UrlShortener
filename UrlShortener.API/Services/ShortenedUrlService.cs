using Microsoft.Extensions.Options;
using UrlShortener.API.Entities;
using UrlShortener.API.Repositories.Interfaces;
using UrlShortener.API.Services.Interfaces;
using UrlShortener.API.Settings;

namespace UrlShortener.API.Services;

public class ShortenedUrlService : IShortenedUrlService
{
    private readonly IInputValidator _inputValidator;
    private readonly IRepository<ShortenedUrl> _shortenedUrlRepository;
    private readonly IShortCodeUrlPrefixer _shortCodeUrlPrefixer;
    private readonly IShortCodeGenerator _shortCodeGenerator;
    private readonly ApplicationSettings _applicationSettings;
    private readonly ILogger<ShortenedUrlService> _logger;

    public ShortenedUrlService(
        IInputValidator inputValidator,
        IRepository<ShortenedUrl> shortenedUrlRepository,
        IShortCodeUrlPrefixer shortCodeUrlPrefixer,
        IShortCodeGenerator shortCodeGenerator,
        IOptions<ApplicationSettings> applicationSettings,
        ILogger<ShortenedUrlService> logger)
    {
        _inputValidator = inputValidator;
        _shortenedUrlRepository = shortenedUrlRepository;
        _shortCodeUrlPrefixer = shortCodeUrlPrefixer;
        _shortCodeGenerator = shortCodeGenerator;
        _applicationSettings = applicationSettings.Value;
        _logger = logger;
    }

    public Uri GetShortenedUrl(string? longUrl)
    {
        _logger.LogInformation("Getting shortened url for '{longUrl}'", longUrl);
        
        if (!_inputValidator.UrlIsValid(longUrl))
            throw new ArgumentException($"{longUrl} is not a valid url");

        var existingShortUrl = _shortenedUrlRepository
            .SearchForSingleOrDefault(x => x.LongUrl == longUrl);

        if (existingShortUrl != null)
            return GetAndLogReturnShortenedUrl(longUrl, existingShortUrl.ShortCode);

        var newShortCode = CreateNewShortCode(longUrl);

        return GetAndLogReturnShortenedUrl(longUrl, newShortCode);
    }

    public Uri? RetrieveLongUrl(string? shortCode)
    {
        _logger.LogInformation("Getting long url for '{shortUrl}'", shortCode);

        if (!_inputValidator.ShortCodeIsValid(shortCode, _applicationSettings.ShortCodeLength))
            throw new ArgumentException(
                $"{shortCode} must not be empty and be {_applicationSettings.ShortCodeLength} characters long");

        var existingShortUrl = _shortenedUrlRepository
            .SearchForSingleOrDefault(x => x.ShortCode == shortCode);
        
        return existingShortUrl != null
            ? new Uri(existingShortUrl.LongUrl)
            : null;
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

    private Uri GetAndLogReturnShortenedUrl(string? longUrl, string shortCode)
    {
        var shortenedUrl = _shortCodeUrlPrefixer.GetPrefixedUrl(shortCode);
        _logger.LogInformation("Returning {shortenedUrl} for {longUrl}", shortenedUrl, longUrl);
        return shortenedUrl;
    }
}