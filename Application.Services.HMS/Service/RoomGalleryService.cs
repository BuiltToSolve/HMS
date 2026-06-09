using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Services.HMS.Database;
using Application.Services.HMS.Service.Interface;

namespace Application.Services.HMS.Service;

public class RoomGalleryService : IRoomGalleryService
{
    private readonly AppDbContext _context;

    public RoomGalleryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoomGallery>> GetAsync(Guid? roomId, GalleryCategory? category)
    {
        var query = _context.RoomGalleries.AsQueryable();

        if (roomId.HasValue)
        {
            query = query.Where(g => g.RoomId == roomId.Value);
        }

        if (category.HasValue)
        {
            query = query.Where(g => g.Category == category.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<RoomGallery?> GetByIdAsync(Guid id)
    {
        return await _context.RoomGalleries.FindAsync(id);
    }

    public async Task<RoomGallery> CreateAsync(RoomGallery roomGallery)
    {
        _context.RoomGalleries.Add(roomGallery);
        await _context.SaveChangesAsync();
        return roomGallery;
    }

    public async Task<IEnumerable<RoomGallery>> UpsertMultipleAsync(IEnumerable<RoomGallery> roomGalleries)
    {
        var galleryIds = roomGalleries.Select(g => g.Id).ToList();
        
        var existingIds = await _context.RoomGalleries
            .Where(g => galleryIds.Contains(g.Id))
            .Select(g => g.Id)
            .ToListAsync();

        foreach (var gallery in roomGalleries)
        {
            if (existingIds.Contains(gallery.Id))
            {
                _context.Entry(gallery).State = EntityState.Modified;
            }
            else
            {
                _context.RoomGalleries.Add(gallery);
            }
        }

        await _context.SaveChangesAsync();
        return roomGalleries;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var gallery = await _context.RoomGalleries.FindAsync(id);
        
        if (gallery == null)
        {
            return false;
        }

        _context.RoomGalleries.Remove(gallery);
        await _context.SaveChangesAsync();
        return true;
    }
}
