using Snapspot.Application.Models.Spots;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Snapspot.Shared.Common;

namespace Snapspot.Application.Services
{
    public interface ISpotService
    {
        Task<SpotDto> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<SpotDto>>> GetAllAsync();
        Task<IEnumerable<SpotDto>> GetByDistrictIdAsync(Guid districtId);
        Task<SpotDto> CreateAsync(CreateSpotDto createSpotDto);
        Task<SpotDto> UpdateAsync(Guid id, UpdateSpotDto updateSpotDto);
        Task<bool> DeleteAsync(Guid id);
    }
} 