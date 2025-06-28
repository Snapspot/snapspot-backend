using Snapspot.Application.Models.Companies;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Interfaces.Company
{
    public interface ICompanyUseCase
    {
        // CRUD Operations
        Task<ApiResponse<CompanyDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<CompanyDto>>> GetAllAsync();
        Task<ApiResponse<CompanyDto>> CreateAsync(CreateCompanyDto createCompanyDto);
        Task<ApiResponse<CompanyDto>> UpdateAsync(Guid id, UpdateCompanyDto updateCompanyDto);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
        
        // Business Operations
        Task<ApiResponse<IEnumerable<CompanyDto>>> SearchCompaniesAsync(string searchTerm);
        Task<ApiResponse<bool>> ExistsAsync(Guid id);
    }
} 