namespace UrlShortener.API.Entities;

public class ShortenedUrl : EntityBase
{
    public string LongUrl { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
}