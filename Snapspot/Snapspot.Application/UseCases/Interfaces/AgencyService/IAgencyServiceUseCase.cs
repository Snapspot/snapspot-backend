using Snapspot.Application.Models.AgencyServices;
using Snapspot.Shared.Common;

namespace Snapspot.Application.UseCases.Interfaces.AgencyService
{
    public interface IAgencyServiceUseCase
    {
        Task<ApiResponse<IEnumerable<AgencyServiceDto>>> GetAllAsync();
        Task<ApiResponse<AgencyServiceDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<AgencyServiceDto>>> GetByAgencyIdAsync(Guid agencyId);
        Task<ApiResponse<AgencyServiceDto>> CreateAsync(CreateAgencyServiceDto createAgencyServiceDto);
        Task<ApiResponse<AgencyServiceDto>> UpdateAsync(Guid id, UpdateAgencyServiceDto updateAgencyServiceDto);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
        Task<ApiResponse<bool>> AddToAgencyAsync(Guid id, Guid agencyId);
        Task<ApiResponse<bool>> RemoveFromAgencyAsync(Guid id, Guid agencyId);
    }
} 