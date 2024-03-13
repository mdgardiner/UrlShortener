using UrlShortener.API.Context;
using UrlShortener.API.Entities;

namespace UrlShortener.API.Repositories;

public class ShortenedUrlRepository : RepositoryBase<ShortenedUrl>
{
    public ShortenedUrlRepository(IShortenedUrlContext dbContext) 
        : base(dbContext)
    {
    }
}