namespace UrlShortener.API.Services.Interfaces;

public interface IInputValidator
{
    bool UrlIsValid(string? url);

    bool ShortCodeIsValid(string? shortCode, int shortCodeLength);
}