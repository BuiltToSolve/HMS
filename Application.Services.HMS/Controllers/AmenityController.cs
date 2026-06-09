using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Services.HMS.Service.Interface;

namespace Application.Services.HMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AmenityController : ControllerBase
{
    private readonly IAmenityService _service;

    public AmenityController(IAmenityService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? roomId)
    {
        var result = await _service.GetAllAsync(roomId ?? Guid.Empty);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Amenity item)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var created = await _service.CreateAsync(item);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Amenity item)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var success = await _service.UpdateAsync(id, item);
        if (!success) return NotFound();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound();
        
        return NoContent();
    }
}
