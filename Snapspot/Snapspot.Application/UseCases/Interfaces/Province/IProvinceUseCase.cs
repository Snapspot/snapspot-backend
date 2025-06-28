using Snapspot.Application.Models.Provinces;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Interfaces.Province
{
    public interface IProvinceUseCase
    {
        Task<ApiResponse<ProvinceDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<ProvinceDto>>> GetAllAsync();
        Task<ApiResponse<ProvinceDto>> CreateAsync(CreateProvinceDto createProvinceDto);
        Task<ApiResponse<ProvinceDto>> UpdateAsync(Guid id, UpdateProvinceDto updateProvinceDto);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
} 