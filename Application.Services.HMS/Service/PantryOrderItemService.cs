using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Services.HMS.Database;
using Application.Services.HMS.Service.Interface;

namespace Application.Services.HMS.Service;

public class PantryOrderItemService : IPantryOrderItemService
{
    private readonly AppDbContext _context;

    public PantryOrderItemService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PantryOrderItem>> GetAllAsync()
    {
        return await _context.PantryOrderItems.ToListAsync();
    }

    public async Task<PantryOrderItem?> GetByIdAsync(Guid id)
    {
        return await _context.PantryOrderItems.FindAsync(id);
    }

    public async Task<PantryOrderItem> CreateAsync(PantryOrderItem entity)
    {
        _context.PantryOrderItems.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Guid id, PantryOrderItem entity)
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
            if (!await _context.PantryOrderItems.AnyAsync(e => e.Id == id)) return false;
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.PantryOrderItems.FindAsync(id);
        if (entity == null) return false;

        entity.Deleted = true;
        await _context.SaveChangesAsync();
        return true;
    }
}
