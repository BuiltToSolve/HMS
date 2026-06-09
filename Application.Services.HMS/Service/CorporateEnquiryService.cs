using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Services.HMS.Database;
using Application.Services.HMS.Service.Interface;

namespace Application.Services.HMS.Service;

public class CorporateEnquiryService : ICorporateEnquiryService
{
    private readonly AppDbContext _context;

    public CorporateEnquiryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CorporateEnquiry>> GetAllAsync()
    {
        return await _context.CorporateEnquiries.ToListAsync();
    }

    public async Task<CorporateEnquiry?> GetByIdAsync(Guid id)
    {
        return await _context.CorporateEnquiries.FindAsync(id);
    }

    public async Task<CorporateEnquiry> CreateAsync(CorporateEnquiry entity)
    {
        _context.CorporateEnquiries.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Guid id, CorporateEnquiry entity)
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
            if (!await _context.CorporateEnquiries.AnyAsync(e => e.Id == id)) return false;
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.CorporateEnquiries.FindAsync(id);
        if (entity == null) return false;

        entity.Deleted = true;
        await _context.SaveChangesAsync();
        return true;
    }
}
