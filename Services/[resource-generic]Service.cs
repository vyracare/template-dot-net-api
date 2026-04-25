using MongoDB.Driver;
using [assembly-generic].Models;

namespace [assembly-generic].Services;

public class [resource-generic]Service
{
    private readonly IMongoCollection<[resource-generic]Model> _collection;

    public [resource-generic]Service(IMongoDatabase db)
    {
        _collection = db.GetCollection<[resource-generic]Model>("[table-generic]");
    }

    public async Task<List<[resource-generic]Model>> GetAllAsync()
    {
        return await _collection.Find(Builders<[resource-generic]Model>.Filter.Empty).ToListAsync();
    }

    public async Task<[resource-generic]Model?> GetByIdAsync(string id)
    {
        return await _collection.Find(item => item.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync([resource-generic]Model item)
    {
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = item.CreatedAt;
        await _collection.InsertOneAsync(item);
    }
}
