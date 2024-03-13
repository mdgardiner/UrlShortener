namespace UrlShortener.API.Services.Interfaces;

public interface IShortCodeGenerator
{
    public string GenerateShortCode(int length);
}