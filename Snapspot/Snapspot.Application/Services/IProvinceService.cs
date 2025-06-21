using Snapspot.Application.Models.Provinces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Snapspot.Shared.Common;

namespace Snapspot.Application.Services
{
    public interface IProvinceService
    {
        Task<ProvinceDto> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<ProvinceDto>>> GetAllAsync();
        Task<ProvinceDto> CreateAsync(CreateProvinceDto createProvinceDto);
        Task<ProvinceDto> UpdateAsync(Guid id, UpdateProvinceDto updateProvinceDto);
        Task<bool> DeleteAsync(Guid id);
    }
} 