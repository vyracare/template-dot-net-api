using [assembly-generic].Common.Results;
using [assembly-generic].Features.[resource-generic].Shared.Domain;
using [assembly-generic].Features.[resource-generic].Shared.Ports;
using ResourceEntity = [assembly-generic].Features.[resource-generic].Shared.Domain.[resource-generic];

namespace [assembly-generic].Features.[resource-generic].GetById;

public sealed class Get[resource-generic]ByIdHandler
{
    private readonly I[resource-generic]Repository _repository;

    public Get[resource-generic]ByIdHandler(I[resource-generic]Repository repository)
    {
        _repository = repository;
    }

    public async Task<UseCaseResult<ResourceEntity>> HandleAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return UseCaseResult<ResourceEntity>.Failure(
                UseCaseErrorType.Validation,
                "O identificador do recurso e obrigatorio.");
        }

        var resource = await _repository.GetByIdAsync(id);
        if (resource is null)
        {
            return UseCaseResult<ResourceEntity>.Failure(
                UseCaseErrorType.NotFound,
                "Recurso nao encontrado.");
        }

        return UseCaseResult<ResourceEntity>.Success(resource);
    }
}
