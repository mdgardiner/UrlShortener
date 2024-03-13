namespace UrlShortener.API.Settings;

public class ApplicationSettings
{
    public string ShortCodeUrlPrefix { get; set; } = string.Empty;
    public int ShortCodeLength { get; set; }
}