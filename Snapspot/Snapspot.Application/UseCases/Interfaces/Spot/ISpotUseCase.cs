using Snapspot.Application.Models.Responses.Spot;
using Snapspot.Application.Models.Spots;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Interfaces.Spot
{
    public interface ISpotUseCase
    {
        // CRUD Operations
        Task<ApiResponse<SpotDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<SpotDto>>> GetAllAsync();
        Task<ApiResponse<SpotDto>> CreateAsync(CreateSpotDto createSpotDto);
        Task<ApiResponse<SpotDto>> UpdateAsync(Guid id, UpdateSpotDto updateSpotDto);
        Task<ApiResponse<string>> DeleteAsync(Guid id);

        Task<ApiResponse<IEnumerable<GetAllSpotWithDistanceReponse>>> GetAllWithDistanceAsync(double ulat, double ulon);
        

            // Business Operations
        Task<ApiResponse<IEnumerable<SpotDto>>> GetByDistrictIdAsync(Guid districtId);
        Task<ApiResponse<IEnumerable<SpotDto>>> GetByProvinceIdAsync(Guid provinceId);
        Task<ApiResponse<IEnumerable<SpotDto>>> SearchSpotsAsync(string searchTerm);
    }
} 