using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.HMS.Service.Interface;

public interface IPantryOrderService
{
    Task<IEnumerable<PantryOrder>> GetAllAsync();
    Task<PantryOrder?> GetByIdAsync(Guid id);
    Task<PantryOrder> CreateAsync(PantryOrder entity);
    Task<bool> UpdateAsync(Guid id, PantryOrder entity);
    Task<bool> DeleteAsync(Guid id);
}
