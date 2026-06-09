using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Services.HMS.Database;
using Application.Services.HMS.Service.Interface;

namespace Application.Services.HMS.Service;

public class ReviewService : IReviewService
{
    private readonly AppDbContext _context;

    public ReviewService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Review>> GetAllAsync()
    {
        return await _context.Reviews.ToListAsync();
    }

    public async Task<Review?> GetByIdAsync(Guid id)
    {
        return await _context.Reviews.FindAsync(id);
    }

    public async Task<Review> CreateAsync(Review entity)
    {
        _context.Reviews.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Guid id, Review entity)
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
            if (!await _context.Reviews.AnyAsync(e => e.Id == id)) return false;
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Reviews.FindAsync(id);
        if (entity == null) return false;

        _context.Reviews.Remove(entity); // hard delete
        await _context.SaveChangesAsync();
        return true;
    }
}
