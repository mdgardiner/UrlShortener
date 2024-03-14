namespace UrlShortener.API.Models.Dtos;

public record ExpandShortenedUrlDto()
{
    public string ShortCode { get; set; } = string.Empty;
};