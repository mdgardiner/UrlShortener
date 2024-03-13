namespace UrlShortener.API.Exceptions;

public class ConfigurationMissingException : ApplicationException
{
    public ConfigurationMissingException(string name)
        : base($"Configuration for {name} is missing.")
    {
    }
}