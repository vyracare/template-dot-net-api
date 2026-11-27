using [assembly-generic].Common.Results;
using [assembly-generic].Features.[resource-generic].Shared.Domain;
using [assembly-generic].Features.[resource-generic].Shared.Ports;
using ResourceEntity = [assembly-generic].Features.[resource-generic].Shared.Domain.[resource-generic];

namespace [assembly-generic].Features.[resource-generic].List;

public sealed class List[resource-generic]Handler
{
    private readonly I[resource-generic]Repository _repository;

    public List[resource-generic]Handler(I[resource-generic]Repository repository)
    {
        _repository = repository;
    }

    public async Task<UseCaseResult<IReadOnlyCollection<ResourceEntity>>> HandleAsync()
    {
        var items = await _repository.ListAsync();
        return UseCaseResult<IReadOnlyCollection<ResourceEntity>>.Success(items);
    }
}
