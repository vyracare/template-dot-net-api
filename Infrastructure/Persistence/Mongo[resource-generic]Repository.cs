using MongoDB.Driver;
using [assembly-generic].Features.[resource-generic].Shared.Domain;
using [assembly-generic].Features.[resource-generic].Shared.Ports;
using [assembly-generic].Infrastructure.Persistence.Documents;

namespace [assembly-generic].Infrastructure.Persistence;

/// <summary>
/// Implementa a integra??o com a persist?ncia ou com uma depend?ncia externa da aplica??o.
/// </summary>
public sealed class Mongo[resource-generic]Repository : I[resource-generic]Repository
{
    private readonly IMongoCollection<[resource-generic]Document> _collection;

    public Mongo[resource-generic]Repository(IMongoDatabase database)
    {
        _collection = database.GetCollection<[resource-generic]Document>("[table-generic]");
    }

    public async Task<[resource-generic]> AddAsync([resource-generic] resource)
    {
        var document = ToDocument(resource);
        await _collection.InsertOneAsync(document);
        resource.Id = document.Id;
        return resource;
    }

    public async Task<[resource-generic]?> GetByIdAsync(string id)
    {
        var document = await _collection.Find(item => item.Id == id).FirstOrDefaultAsync();
        return document is null ? null : ToDomain(document);
    }

    public async Task<IReadOnlyCollection<[resource-generic]>> ListAsync()
    {
        var documents = await _collection.Find(_ => true).ToListAsync();
        return documents.Select(ToDomain).ToArray();
    }

    private static [resource-generic]Document ToDocument([resource-generic] resource) =>
        new()
        {
            Id = resource.Id,
            Name = resource.Name,
            Code = resource.Code,
            Description = resource.Description,
            IsActive = resource.IsActive,
            CreatedAt = resource.CreatedAt,
            UpdatedAt = resource.UpdatedAt
        };

    private static [resource-generic] ToDomain([resource-generic]Document document) =>
        new()
        {
            Id = document.Id,
            Name = document.Name,
            Code = document.Code,
            Description = document.Description,
            IsActive = document.IsActive,
            CreatedAt = document.CreatedAt,
            UpdatedAt = document.UpdatedAt
        };
}
