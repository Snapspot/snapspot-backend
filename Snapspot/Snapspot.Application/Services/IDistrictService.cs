using Snapspot.Application.Models.Districts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Services
{
    public interface IDistrictService
    {
        Task<DistrictDto> GetByIdAsync(Guid id);
        Task<IEnumerable<DistrictDto>> GetAllAsync();
        Task<IEnumerable<DistrictDto>> GetByProvinceIdAsync(Guid provinceId);
        Task<DistrictDto> CreateAsync(CreateDistrictDto createDistrictDto);
        Task<DistrictDto> UpdateAsync(Guid id, UpdateDistrictDto updateDistrictDto);
        Task<bool> DeleteAsync(Guid id);
    }
} 