using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Services.HMS.Database;
using Application.Services.HMS.Service.Interface;

namespace Application.Services.HMS.Service;

public class RoomService : IRoomService
{
    private readonly AppDbContext _context;

    public RoomService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Room>> GetAsync(RoomType? type, bool? isActive)
    {
        var query = _context.Rooms.AsQueryable();

        if (type.HasValue)
        {
            query = query.Where(r => r.Type == type.Value);
        }

        if (isActive.HasValue)
        {
            query = query.Where(r => r.IsActive == isActive.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Room?> GetByIdAsync(Guid id)
    {
        var room = await _context.Rooms.FindAsync(id);
        return room;
    }

    public async Task<Room> CreateAsync(Room room)
    {
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task UpdateAsync(Guid id, Room room)
    {
        _context.Entry(room).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateStatusAsync(Guid id, RoomStatusUpdateDto statusUpdate)
    {
        var room = await _context.Rooms.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.Id == id);
        
        if (room == null)
        {
            return false;
        }

        if (statusUpdate.IsActive.HasValue)
        {
            room.IsActive = statusUpdate.IsActive.Value;
        }

        if (statusUpdate.Deleted.HasValue)
        {
            room.Deleted = statusUpdate.Deleted.Value;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var room = await _context.Rooms.FindAsync(id);
        
        if (room == null)
        {
            return false;
        }

        room.Deleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public bool RoomExists(Guid id)
    {
        return _context.Rooms.Any(e => e.Id == id);
    }
}
