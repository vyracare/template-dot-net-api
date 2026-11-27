using [assembly-generic].Common.Results;
using [assembly-generic].Common.Time;
using [assembly-generic].Features.[resource-generic].Shared.Domain;
using [assembly-generic].Features.[resource-generic].Shared.Ports;
using ResourceEntity = [assembly-generic].Features.[resource-generic].Shared.Domain.[resource-generic];

namespace [assembly-generic].Features.[resource-generic].Create;

public sealed class Create[resource-generic]Handler
{
    private readonly I[resource-generic]Repository _repository;
    private readonly IClock _clock;

    public Create[resource-generic]Handler(I[resource-generic]Repository repository, IClock clock)
    {
        _repository = repository;
        _clock = clock;
    }

    public async Task<UseCaseResult<ResourceEntity>> HandleAsync(Create[resource-generic]Request request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return UseCaseResult<ResourceEntity>.Failure(
                UseCaseErrorType.Validation,
                "O nome do recurso e obrigatorio.");
        }

        var timestamp = _clock.UtcNow;
        var resource = new ResourceEntity
        {
            Name = request.Name.Trim(),
            Code = string.IsNullOrWhiteSpace(request.Code) ? null : request.Code.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            IsActive = request.IsActive,
            CreatedAt = timestamp,
            UpdatedAt = timestamp
        };

        var created = await _repository.AddAsync(resource);
        return UseCaseResult<ResourceEntity>.Success(created);
    }
}
