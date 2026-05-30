using [assembly-generic].Features.[resource-generic].Shared.Domain;

namespace [assembly-generic].Features.[resource-generic].Shared.Ports;

public interface I[resource-generic]Repository
{
    Task<[resource-generic]> AddAsync([resource-generic] resource);
    Task<[resource-generic]?> GetByIdAsync(string id);
    Task<IReadOnlyCollection<[resource-generic]>> ListAsync();
}
