using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Context;

namespace UrlShortener.API.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ShortenedUrlContext dbContext =
            scope.ServiceProvider.GetRequiredService<ShortenedUrlContext>();

        dbContext.Database.Migrate();
    }
}