using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Services.HMS.Database;
using Application.Services.HMS.Service.Interface;

namespace Application.Services.HMS.Service;

public class PantryOrderService : IPantryOrderService
{
    private readonly AppDbContext _context;

    public PantryOrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PantryOrder>> GetAllAsync()
    {
        return await _context.PantryOrders
            .Include(o => o.Booking)
            .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
            .ToListAsync();
    }

    public async Task<PantryOrder?> GetByIdAsync(Guid id)
    {
        return await _context.PantryOrders
            .Include(o => o.Booking)
            .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<PantryOrder> CreateAsync(PantryOrder entity)
    {
        _context.PantryOrders.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Guid id, PantryOrder entity)
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
            if (!await _context.PantryOrders.AnyAsync(e => e.Id == id)) return false;
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.PantryOrders.FindAsync(id);
        if (entity == null) return false;

        entity.Deleted = true;
        await _context.SaveChangesAsync();
        return true;
    }
}
