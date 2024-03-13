namespace UrlShortener.API.Models.Dtos;

public record CreateShortenedUrlDto()
{
    public string LongUrl { get; set; } = string.Empty;
};