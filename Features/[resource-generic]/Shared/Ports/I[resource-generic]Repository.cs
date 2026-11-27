using [assembly-generic].Features.[resource-generic].Shared.Domain;
using ResourceEntity = [assembly-generic].Features.[resource-generic].Shared.Domain.[resource-generic];

namespace [assembly-generic].Features.[resource-generic].Shared.Ports;

public interface I[resource-generic]Repository
{
    Task<ResourceEntity> AddAsync(ResourceEntity resource);
    Task<ResourceEntity?> GetByIdAsync(string id);
    Task<IReadOnlyCollection<ResourceEntity>> ListAsync();
}
