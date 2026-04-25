using Microsoft.AspNetCore.Mvc;
using [assembly-generic].DTOS;
using [assembly-generic].Models;
using [assembly-generic].Services;

namespace [assembly-generic].Controllers;

[ApiController]
[Route("api/[table-route-generic]")]
public class [resource-generic]Controller : ControllerBase
{
    private readonly [resource-generic]Service _service;

    public [resource-generic]Controller([resource-generic]Service service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var item = await _service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Create[resource-generic]Request request)
    {
        var item = new [resource-generic]Model
        {
            Name = request.Name,
            Description = request.Description
        };

        await _service.CreateAsync(item);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }
}
