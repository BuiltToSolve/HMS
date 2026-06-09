using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.HMS.Database;

namespace Application.Services.HMS.Service.Interface;

public interface IRoomGalleryService
{
    Task<IEnumerable<RoomGallery>> GetAsync(Guid? roomId, GalleryCategory? category);
    Task<RoomGallery?> GetByIdAsync(Guid id);
    Task<RoomGallery> CreateAsync(RoomGallery roomGallery);
    Task<IEnumerable<RoomGallery>> UpsertMultipleAsync(IEnumerable<RoomGallery> roomGalleries);
    Task<bool> DeleteAsync(Guid id);
}
