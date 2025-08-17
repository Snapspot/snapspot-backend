using Snapspot.Application.Models.Agencies;
using Snapspot.Application.Models.Responses.Agency;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Interfaces.Agency
{
    public interface IAgencyUseCase
    {
        // CRUD Operations
        Task<ApiResponse<AgencyDto>> GetByIdAsync(Guid id, string? userId);
        Task<ApiResponse<IEnumerable<AgencyDto>>> GetAllAsync();
        Task<ApiResponse<AgencyCreationResponse>> CreateAsync(CreateAgencyDto createAgencyDto, string? userId);
        Task<ApiResponse<AgencyDto>> UpdateAsync(Guid id, UpdateAgencyDto updateAgencyDto);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
        
        // Business Operations
        Task<ApiResponse<IEnumerable<AgencyDto>>> GetByCompanyIdAsync(Guid companyId);
        Task<ApiResponse<IEnumerable<AgencyDto>>> GetBySpotIdAsync(Guid spotId);
        Task<ApiResponse<bool>> ExistsAsync(Guid id);
        Task<ApiResponse<IEnumerable<AgencyDto>>> SearchAgenciesAsync(string searchTerm);
    }
} 