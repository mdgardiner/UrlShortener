using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Entities;

namespace UrlShortener.API.Context;

public class ShortenedUrlContext : DbContext, IShortenedUrlContext
{
    public ShortenedUrlContext(DbContextOptions<ShortenedUrlContext> options)
        : base(options)
    {
    }
    
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        //Database.Migrate();
    }
}