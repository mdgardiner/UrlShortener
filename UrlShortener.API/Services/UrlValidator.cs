using UrlShortener.API.Services.Interfaces;

namespace UrlShortener.API.Services;

public class UrlValidator : IUrlValidator
{
    public bool IsValid(string? url)
        => !string.IsNullOrWhiteSpace(url) &&
           Uri.TryCreate(url, UriKind.Absolute, out _);
}