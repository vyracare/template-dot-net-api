using [assembly-generic].Features.[resource-generic].Shared.Domain;

namespace [assembly-generic].Features.[resource-generic].Shared.Ports;

/// <summary>
/// Implementa a integra??o com a persist?ncia ou com uma depend?ncia externa da aplica??o.
/// </summary>
public interface I[resource-generic]Repository
{
    Task<[resource-generic]> AddAsync([resource-generic] resource);
    Task<[resource-generic]?> GetByIdAsync(string id);
    Task<IReadOnlyCollection<[resource-generic]>> ListAsync();
}
