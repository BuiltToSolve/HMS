using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Services.HMS.Database;
using Application.Services.HMS.Service.Interface;

namespace Application.Services.HMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomGalleryController : ControllerBase
{
    private readonly IRoomGalleryService _roomGalleryService;

    public RoomGalleryController(IRoomGalleryService roomGalleryService)
    {
        _roomGalleryService = roomGalleryService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid? roomId, [FromQuery] GalleryCategory? category)
    {
        var galleries = await _roomGalleryService.GetAsync(roomId, category);
        return Ok(galleries);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var gallery = await _roomGalleryService.GetByIdAsync(id);
        
        if (gallery == null)
        {
            return NotFound();
        }

        return Ok(gallery);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RoomGallery roomGallery)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdGallery = await _roomGalleryService.CreateAsync(roomGallery);

        return CreatedAtAction(nameof(GetById), new { id = createdGallery.Id }, createdGallery);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] List<RoomGallery> roomGalleries)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (roomGalleries == null || roomGalleries.Count == 0)
        {
            return BadRequest("No items to update.");
        }

        var upsertedGalleries = await _roomGalleryService.UpsertMultipleAsync(roomGalleries);
        
        return Ok(upsertedGalleries);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _roomGalleryService.DeleteAsync(id);
        
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
