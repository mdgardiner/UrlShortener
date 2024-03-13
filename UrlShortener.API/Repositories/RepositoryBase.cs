using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Context;
using UrlShortener.API.Entities.Interfaces;
using UrlShortener.API.Repositories.Interfaces;

namespace UrlShortener.API.Repositories;

public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    protected readonly DbContext DbContext;

    public RepositoryBase(IShortenedUrlContext dbContext)
    {
        DbContext = (DbContext)dbContext;
    }

    protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();
    
    public void Insert(TEntity entity)
    { 
        DbSet.Add(entity);
    }

    public TEntity? SearchForSingleOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return DbSet.Local.AsQueryable().SingleOrDefault(predicate) ?? DbSet.SingleOrDefault(predicate);
    }

    public int Commit()
    {
        return DbContext.SaveChanges();
    }
}