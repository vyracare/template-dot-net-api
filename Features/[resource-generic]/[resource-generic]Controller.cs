using Microsoft.AspNetCore.Mvc;
using [assembly-generic].Common.Http;
using [assembly-generic].Features.[resource-generic].Create;
using [assembly-generic].Features.[resource-generic].GetById;
using [assembly-generic].Features.[resource-generic].List;

namespace [assembly-generic].Features.[resource-generic];

[ApiController]
[Route("api/[table-route-generic]")]
public sealed class [resource-generic]Controller : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromServices] List[resource-generic]Handler handler)
    {
        var result = await handler.HandleAsync();
        return this.ToActionResult(result, Ok);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, [FromServices] Get[resource-generic]ByIdHandler handler)
    {
        var result = await handler.HandleAsync(id);
        return this.ToActionResult(result, Ok);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] Create[resource-generic]Request request,
        [FromServices] Create[resource-generic]Handler handler)
    {
        var result = await handler.HandleAsync(request);
        return this.ToActionResult(result, value => CreatedAtAction(nameof(GetById), new { id = value.Id }, value));
    }
}
