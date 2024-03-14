using UrlShortener.API.Services.Interfaces;

namespace UrlShortener.API.Services;

public class InputValidator : IInputValidator
{
    public bool UrlIsValid(string? url)
        => !string.IsNullOrWhiteSpace(url) &&
           Uri.TryCreate(url, UriKind.Absolute, out _);

    public bool ShortCodeIsValid(string? shortCode, int shortCodeLength)
        => !(string.IsNullOrWhiteSpace(shortCode) ||
           shortCode.Length != shortCodeLength);

}