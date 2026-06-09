using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.HMS.Service.Interface;

public interface IBookingService
{
    Task<IEnumerable<Booking>> GetAllAsync();
    Task<Booking?> GetByIdAsync(Guid id);
    Task<Booking> CreateAsync(Booking booking);
    Task<bool> UpdateAsync(Guid id, Booking booking);
    Task<bool> DeleteAsync(Guid id);
}
