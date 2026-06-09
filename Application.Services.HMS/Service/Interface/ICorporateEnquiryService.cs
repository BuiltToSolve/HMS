using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.HMS.Service.Interface;

public interface ICorporateEnquiryService
{
    Task<IEnumerable<CorporateEnquiry>> GetAllAsync();
    Task<CorporateEnquiry?> GetByIdAsync(Guid id);
    Task<CorporateEnquiry> CreateAsync(CorporateEnquiry entity);
    Task<bool> UpdateAsync(Guid id, CorporateEnquiry entity);
    Task<bool> DeleteAsync(Guid id);
}
