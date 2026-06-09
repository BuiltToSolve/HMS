using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.HMS.Service.Interface;

public interface IMenuItemService
{
    Task<IEnumerable<MenuItem>> GetAllAsync();
    Task<MenuItem?> GetByIdAsync(Guid id);
    Task<MenuItem> CreateAsync(MenuItem entity);
    Task<bool> UpdateAsync(Guid id, MenuItem entity);
    Task<bool> DeleteAsync(Guid id);
}
