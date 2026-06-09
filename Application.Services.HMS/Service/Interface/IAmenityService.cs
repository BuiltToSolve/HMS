using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.HMS.Service.Interface;

public interface IAmenityService
{
    Task<IEnumerable<Amenity>> GetAllAsync(Guid? roomId);
    Task<Amenity?> GetByIdAsync(Guid id);
    Task<Amenity> CreateAsync(Amenity amenity);
    Task<bool> UpdateAsync(Guid id, Amenity amenity);
    Task<bool> DeleteAsync(Guid id);
}
