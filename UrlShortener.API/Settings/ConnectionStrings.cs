namespace UrlShortener.API.Settings;

public class ConnectionStrings
{
    public ConnectionStrings()
    {
        Database = String.Empty;
    }

    public string Database { get; set; }
}