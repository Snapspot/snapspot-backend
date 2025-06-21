using Snapspot.Application.Models.Districts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Snapspot.Shared.Common;

namespace Snapspot.Application.Services
{
    public interface IDistrictService
    {
        Task<DistrictDto> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<DistrictDto>>> GetAllAsync();
        Task<IEnumerable<DistrictDto>> GetByProvinceIdAsync(Guid provinceId);
        Task<DistrictDto> CreateAsync(CreateDistrictDto createDistrictDto);
        Task<DistrictDto> UpdateAsync(Guid id, UpdateDistrictDto updateDistrictDto);
        Task<bool> DeleteAsync(Guid id);
    }
} 