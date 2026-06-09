using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.HMS.Service.Interface;

public interface IPantryOrderItemService
{
    Task<IEnumerable<PantryOrderItem>> GetAllAsync();
    Task<PantryOrderItem?> GetByIdAsync(Guid id);
    Task<PantryOrderItem> CreateAsync(PantryOrderItem entity);
    Task<bool> UpdateAsync(Guid id, PantryOrderItem entity);
    Task<bool> DeleteAsync(Guid id);
}
