using [assembly-generic].Common.Results;
using [assembly-generic].Common.Time;
using [assembly-generic].Features.[resource-generic].Create;
using [assembly-generic].Features.[resource-generic].Shared.Domain;
using [assembly-generic].Features.[resource-generic].Shared.Ports;

namespace [assembly-generic].Tests.[resource-generic].Create;

/// <summary>
/// Agrupa os cen?rios de teste unit?rio relacionados a este componente.
/// </summary>
public sealed class Create[resource-generic]HandlerTests
{
    [Fact]
/// <summary>
/// Executa a responsabilidade associada a d ev e r et or na r v al id ac ao q ua nd o n om e n ao f or i nf or ma do.
/// </summary>
    public async Task Deve_retornar_validacao_quando_nome_nao_for_informado()
    {
        var handler = new Create[resource-generic]Handler(new FakeRepository(), new FixedClock());

        var result = await handler.HandleAsync(new Create[resource-generic]Request("", "COD-001", "Descricao", true));

        Assert.False(result.IsSuccess);
        Assert.Equal(UseCaseErrorType.Validation, result.ErrorType);
    }

    [Fact]
/// <summary>
/// Executa a responsabilidade associada a d ev e c ri ar r ec ur so q ua nd o p ay lo ad f or v al id o.
/// </summary>
    public async Task Deve_criar_recurso_quando_payload_for_valido()
    {
        var repository = new FakeRepository();
        var handler = new Create[resource-generic]Handler(repository, new FixedClock());

        var result = await handler.HandleAsync(new Create[resource-generic]Request("Item", "COD-001", "Descricao", true));

        Assert.True(result.IsSuccess);
        Assert.Single(repository.Items);
    }

    private sealed class FakeRepository : I[resource-generic]Repository
    {
        public List<[resource-generic]> Items { get; } = [];

        public Task<[resource-generic]> AddAsync([resource-generic] resource)
        {
            resource.Id ??= Guid.NewGuid().ToString("N");
            Items.Add(resource);
            return Task.FromResult(resource);
        }

        public Task<[resource-generic]?> GetByIdAsync(string id) => Task.FromResult(Items.FirstOrDefault(item => item.Id == id));

        public Task<IReadOnlyCollection<[resource-generic]>> ListAsync() => Task.FromResult<IReadOnlyCollection<[resource-generic]>>(Items);
    }

    private sealed class FixedClock : IClock
    {
        public DateTime UtcNow => new(2026, 5, 30, 12, 0, 0, DateTimeKind.Utc);
    }
}
