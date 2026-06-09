using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.HMS.Service.Interface;

public interface IRoomPriceModifierService
{
    Task<IEnumerable<RoomPriceModifier>> GetAllAsync();
    Task<RoomPriceModifier?> GetByIdAsync(Guid id);
    Task<RoomPriceModifier> CreateAsync(RoomPriceModifier entity);
    Task<bool> UpdateAsync(Guid id, RoomPriceModifier entity);
    Task<bool> DeleteAsync(Guid id);
}
