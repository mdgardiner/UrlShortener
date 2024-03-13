using System.Linq.Expressions;
using UrlShortener.API.Entities.Interfaces;

namespace UrlShortener.API.Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : class, IEntity
{
    void Insert(TEntity entity);
    
    TEntity? SearchForSingleOrDefault(Expression<Func<TEntity, bool>> predicate);

    int Commit();
}