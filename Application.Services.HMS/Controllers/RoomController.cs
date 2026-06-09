using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Services.HMS.Database;
using Application.Services.HMS.Service.Interface;

namespace Application.Services.HMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] RoomType? type, [FromQuery] bool? isActive)
    {
        var rooms = await _roomService.GetAsync(type, isActive);
        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var room = await _roomService.GetByIdAsync(id);
        
        if (room == null)
        {
            return NotFound();
        }

        return Ok(room);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Room room)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdRoom = await _roomService.CreateAsync(room);

        return CreatedAtAction(nameof(GetById), new { id = createdRoom.Id }, createdRoom);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] Room room)
    {
        if (id != room.Id)
        {
            return BadRequest("ID in URL does not match ID in the request body.");
        }

        try
        {
            await _roomService.UpdateAsync(id, room);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_roomService.RoomExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> PatchStatus(Guid id, [FromBody] RoomStatusUpdateDto statusUpdate)
    {
        var success = await _roomService.UpdateStatusAsync(id, statusUpdate);
        
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _roomService.DeleteAsync(id);
        
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
