using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.HMS.Service.Interface;

public interface IReviewService
{
    Task<IEnumerable<Review>> GetAllAsync();
    Task<Review?> GetByIdAsync(Guid id);
    Task<Review> CreateAsync(Review entity);
    Task<bool> UpdateAsync(Guid id, Review entity);
    Task<bool> DeleteAsync(Guid id);
}
