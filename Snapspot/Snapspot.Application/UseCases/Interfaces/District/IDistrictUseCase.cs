using Snapspot.Application.Models.Districts;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Interfaces.District
{
    public interface IDistrictUseCase
    {
        Task<ApiResponse<DistrictDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<DistrictDto>>> GetAllAsync();
        Task<ApiResponse<IEnumerable<DistrictDto>>> GetByProvinceIdAsync(Guid provinceId);
        Task<ApiResponse<DistrictDto>> CreateAsync(CreateDistrictDto createDistrictDto);
        Task<ApiResponse<DistrictDto>> UpdateAsync(Guid id, UpdateDistrictDto updateDistrictDto);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
} 