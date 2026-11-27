using [assembly-generic].Common.Results;
using [assembly-generic].Features.[resource-generic].GetById;
using [assembly-generic].Features.[resource-generic].Shared.Domain;
using [assembly-generic].Features.[resource-generic].Shared.Ports;
using ResourceEntity = [assembly-generic].Features.[resource-generic].Shared.Domain.[resource-generic];

namespace [assembly-generic].Tests.[resource-generic].GetById;

public sealed class Get[resource-generic]ByIdHandlerTests
{
    [Fact]
    public async Task Deve_retornar_not_found_quando_recurso_nao_existir()
    {
        var handler = new Get[resource-generic]ByIdHandler(new FakeRepository());

        var result = await handler.HandleAsync("missing-id");

        Assert.False(result.IsSuccess);
        Assert.Equal(UseCaseErrorType.NotFound, result.ErrorType);
    }

    [Fact]
    public async Task Deve_retornar_recurso_quando_identificador_existir()
    {
        var repository = new FakeRepository();
        await repository.AddAsync(new ResourceEntity
        {
            Id = "resource-1",
            Name = "Item"
        });

        var handler = new Get[resource-generic]ByIdHandler(repository);

        var result = await handler.HandleAsync("resource-1");

        Assert.True(result.IsSuccess);
        Assert.Equal("resource-1", result.Value!.Id);
    }

    private sealed class FakeRepository : I[resource-generic]Repository
    {
        private readonly List<ResourceEntity> _items = [];

        public Task<ResourceEntity> AddAsync(ResourceEntity resource)
        {
            resource.Id ??= Guid.NewGuid().ToString("N");
            _items.Add(resource);
            return Task.FromResult(resource);
        }

        public Task<ResourceEntity?> GetByIdAsync(string id)
        {
            return Task.FromResult(_items.FirstOrDefault(item => item.Id == id));
        }

        public Task<IReadOnlyCollection<ResourceEntity>> ListAsync()
        {
            return Task.FromResult<IReadOnlyCollection<ResourceEntity>>(_items);
        }
    }
}
