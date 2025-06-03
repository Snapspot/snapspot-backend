using Snapspot.Application.Models.Companies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Services
{
    public interface ICompanyService
    {
        Task<CompanyDto> GetByIdAsync(Guid id);
        Task<IEnumerable<CompanyDto>> GetAllAsync();
        Task<CompanyDto> GetByUserIdAsync(Guid userId);
        Task<CompanyDto> CreateAsync(CreateCompanyDto createCompanyDto);
        Task<CompanyDto> UpdateAsync(Guid id, UpdateCompanyDto updateCompanyDto);
        Task<bool> DeleteAsync(Guid id);
    }
} 