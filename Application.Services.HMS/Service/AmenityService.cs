using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Services.HMS.Database;
using Application.Services.HMS.Service.Interface;

namespace Application.Services.HMS.Service;

public class AmenityService : IAmenityService
{
    private readonly AppDbContext _context;

    public AmenityService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Amenity>> GetAllAsync(Guid? roomId)
    {
        IQueryable<Amenity> query = _context.Amenities.AsQueryable();
        if(roomId.HasValue && roomId != Guid.Empty) {
            query = query.Where(a => a.RoomId == roomId);
        }
        return await query.ToListAsync();
    }

    public async Task<Amenity?> GetByIdAsync(Guid id)
    {
        return await _context.Amenities.FindAsync(id);
    }

    public async Task<Amenity> CreateAsync(Amenity amenity)
    {
        _context.Amenities.Add(amenity);
        await _context.SaveChangesAsync();
        return amenity;
    }

    public async Task<bool> UpdateAsync(Guid id, Amenity amenity)
    {
        if (id != amenity.Id) return false;
        
        _context.Entry(amenity).State = EntityState.Modified;
        
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Amenities.AnyAsync(e => e.Id == id))
            {
                return false;
            }
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var amenity = await _context.Amenities.FindAsync(id);
        if (amenity == null) return false;

        // Hard delete since Deleted column was removed
        _context.Amenities.Remove(amenity);
        await _context.SaveChangesAsync();
        return true;
    }
}
