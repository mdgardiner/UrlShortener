using UrlShortener.API.Entities.Interfaces;

namespace UrlShortener.API.Entities;

public class EntityBase : IEntity
{
    public int Id { get; set; }
}