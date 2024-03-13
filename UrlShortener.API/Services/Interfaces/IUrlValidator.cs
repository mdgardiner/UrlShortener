namespace UrlShortener.API.Services.Interfaces;

public interface IUrlValidator
{
    bool IsValid(string? url);

}