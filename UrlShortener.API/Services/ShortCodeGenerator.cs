using UrlShortener.API.Services.Interfaces;

namespace UrlShortener.API.Services;

public class ShortCodeGenerator : IShortCodeGenerator
{
    private readonly Random _random = new();
    private const string PermittedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
    
    public string GenerateShortCode(int length)
    {
        var characters = Enumerable.Range(0, length)
            .Select(x => PermittedCharacters[_random.Next(PermittedCharacters.Length)])
            .ToArray();
        
        return new string(characters);
    }
}