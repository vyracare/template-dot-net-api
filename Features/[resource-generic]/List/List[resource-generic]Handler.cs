using [assembly-generic].Common.Results;
using [assembly-generic].Features.[resource-generic].Shared.Domain;
using [assembly-generic].Features.[resource-generic].Shared.Ports;

namespace [assembly-generic].Features.[resource-generic].List;

public sealed class List[resource-generic]Handler
{
    private readonly I[resource-generic]Repository _repository;

    public List[resource-generic]Handler(I[resource-generic]Repository repository)
    {
        _repository = repository;
    }

    public async Task<UseCaseResult<IReadOnlyCollection<[resource-generic]>>> HandleAsync()
    {
        var items = await _repository.ListAsync();
        return UseCaseResult<IReadOnlyCollection<[resource-generic]>>.Success(items);
    }
}
