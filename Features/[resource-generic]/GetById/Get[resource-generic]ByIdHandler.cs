using [assembly-generic].Common.Results;
using [assembly-generic].Features.[resource-generic].Shared.Domain;
using [assembly-generic].Features.[resource-generic].Shared.Ports;

namespace [assembly-generic].Features.[resource-generic].GetById;

/// <summary>
/// Implementa a regra de neg?cio do caso de uso representado por esta pasta.
/// </summary>
public sealed class Get[resource-generic]ByIdHandler
{
    private readonly I[resource-generic]Repository _repository;

    public Get[resource-generic]ByIdHandler(I[resource-generic]Repository repository)
    {
        _repository = repository;
    }

    public async Task<UseCaseResult<[resource-generic]>> HandleAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return UseCaseResult<[resource-generic]>.Failure(
                UseCaseErrorType.Validation,
                "O identificador do recurso e obrigatorio.");
        }

        var resource = await _repository.GetByIdAsync(id);
        if (resource is null)
        {
            return UseCaseResult<[resource-generic]>.Failure(
                UseCaseErrorType.NotFound,
                "Recurso nao encontrado.");
        }

        return UseCaseResult<[resource-generic]>.Success(resource);
    }
}
