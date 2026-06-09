using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Services.HMS.Database;
using Application.Services.HMS.Service.Interface;

namespace Application.Services.HMS.Service;

public class RoomPriceModifierService : IRoomPriceModifierService
{
    private readonly AppDbContext _context;

    public RoomPriceModifierService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoomPriceModifier>> GetAllAsync()
    {
        return await _context.RoomPriceModifiers.ToListAsync();
    }

    public async Task<RoomPriceModifier?> GetByIdAsync(Guid id)
    {
        return await _context.RoomPriceModifiers.FindAsync(id);
    }

    public async Task<RoomPriceModifier> CreateAsync(RoomPriceModifier entity)
    {
        _context.RoomPriceModifiers.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Guid id, RoomPriceModifier entity)
    {
        if (id != entity.Id) return false;
        
        _context.Entry(entity).State = EntityState.Modified;
        
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.RoomPriceModifiers.AnyAsync(e => e.Id == id)) return false;
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.RoomPriceModifiers.FindAsync(id);
        if (entity == null) return false;

        _context.RoomPriceModifiers.Remove(entity); // hard delete
        await _context.SaveChangesAsync();
        return true;
    }
}
