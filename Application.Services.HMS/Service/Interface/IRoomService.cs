using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.HMS.Database;

namespace Application.Services.HMS.Service.Interface;

public interface IRoomService
{
    Task<IEnumerable<Room>> GetAsync(RoomType? type, bool? isActive);
    Task<Room?> GetByIdAsync(Guid id);
    Task<Room> CreateAsync(Room room);
    Task UpdateAsync(Guid id, Room room);
    Task<bool> UpdateStatusAsync(Guid id, RoomStatusUpdateDto statusUpdate);
    Task<bool> DeleteAsync(Guid id);
    bool RoomExists(Guid id);
}

public class RoomStatusUpdateDto
{
    public bool? IsActive { get; set; }
    public bool? Deleted { get; set; }
}
