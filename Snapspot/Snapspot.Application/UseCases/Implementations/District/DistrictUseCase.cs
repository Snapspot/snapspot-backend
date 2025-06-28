using Snapspot.Application.Models.Districts;
using Snapspot.Application.Repositories;
using Snapspot.Application.UseCases.Interfaces.District;
using Snapspot.Domain.Entities;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Implementations.District
{
    public class DistrictUseCase : IDistrictUseCase
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly IProvinceRepository _provinceRepository;

        public DistrictUseCase(
            IDistrictRepository districtRepository,
            IProvinceRepository provinceRepository)
        {
            _districtRepository = districtRepository;
            _provinceRepository = provinceRepository;
        }

        public async Task<ApiResponse<DistrictDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var district = await _districtRepository.GetByIdAsync(id);
                if (district == null)
                {
                    return new ApiResponse<DistrictDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "District not found"
                    };
                }

                var districtDto = new DistrictDto
                {
                    Id = district.Id,
                    Name = district.Name,
                    Description = district.Description,
                    ProvinceId = district.ProvinceId,
                    ProvinceName = district.Province?.Name ?? "",
                    CreatedAt = district.CreatedAt,
                    UpdatedAt = district.UpdatedAt,
                    IsDeleted = district.IsDeleted
                };

                return new ApiResponse<DistrictDto>
                {
                    Data = districtDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<DistrictDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<DistrictDto>>> GetAllAsync()
        {
            try
            {
                var districts = await _districtRepository.GetAllAsync();
                
                var districtDtos = districts.Select(d => new DistrictDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    ProvinceId = d.ProvinceId,
                    ProvinceName = d.Province?.Name ?? "",
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt,
                    IsDeleted = d.IsDeleted
                }).ToList();

                return new ApiResponse<IEnumerable<DistrictDto>>
                {
                    Data = districtDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<DistrictDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<DistrictDto>>> GetByProvinceIdAsync(Guid provinceId)
        {
            try
            {
                var districts = await _districtRepository.GetByProvinceIdAsync(provinceId);
                
                var districtDtos = districts.Select(d => new DistrictDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    ProvinceId = d.ProvinceId,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt
                }).ToList();

                return new ApiResponse<IEnumerable<DistrictDto>>
                {
                    Data = districtDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<DistrictDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<DistrictDto>> CreateAsync(CreateDistrictDto createDistrictDto)
        {
            try
            {
                // Business logic: Validate province exists
                var province = await _provinceRepository.GetByIdAsync(createDistrictDto.ProvinceId);
                if (province == null)
                {
                    return new ApiResponse<DistrictDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Province not found"
                    };
                }

                // Business logic: Check if district name already exists in the same province
                var allDistricts = await _districtRepository.GetAllAsync();
                var existingDistrict = allDistricts.FirstOrDefault(x => x.Name == createDistrictDto.Name && x.ProvinceId == createDistrictDto.ProvinceId);
                if (existingDistrict != null)
                {
                    return new ApiResponse<DistrictDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "A district with this name already exists in this province"
                    };
                }

                var district = new Domain.Entities.District
                {
                    Id = Guid.NewGuid(),
                    Name = createDistrictDto.Name,
                    Description = createDistrictDto.Description,
                    ProvinceId = createDistrictDto.ProvinceId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _districtRepository.AddAsync(district);
                await _districtRepository.SaveChangesAsync();

                var districtDto = new DistrictDto
                {
                    Id = district.Id,
                    Name = district.Name,
                    Description = district.Description,
                    ProvinceId = district.ProvinceId,
                    ProvinceName = province.Name,
                    CreatedAt = district.CreatedAt,
                    UpdatedAt = district.UpdatedAt,
                    IsDeleted = district.IsDeleted
                };

                return new ApiResponse<DistrictDto>
                {
                    Data = districtDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<DistrictDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<DistrictDto>> UpdateAsync(Guid id, UpdateDistrictDto updateDistrictDto)
        {
            try
            {
                var district = await _districtRepository.GetByIdAsync(id);
                if (district == null)
                {
                    return new ApiResponse<DistrictDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "District not found"
                    };
                }

                // Business logic: Validate province exists if changing
                if (updateDistrictDto.ProvinceId != district.ProvinceId)
                {
                    var province = await _provinceRepository.GetByIdAsync(updateDistrictDto.ProvinceId);
                    if (province == null)
                    {
                        return new ApiResponse<DistrictDto>
                        {
                            Success = false,
                            MessageId = MessageId.E0000,
                            Message = "Province not found"
                        };
                    }
                }

                // Business logic: Check if district name already exists in the same province
                if (!string.IsNullOrEmpty(updateDistrictDto.Name) && updateDistrictDto.Name != district.Name)
                {
                    var provinceId = updateDistrictDto.ProvinceId;
                    var allDistricts = await _districtRepository.GetAllAsync();
                    var existingDistrict = allDistricts.FirstOrDefault(x => x.Name == updateDistrictDto.Name && x.ProvinceId == provinceId);
                    if (existingDistrict != null && existingDistrict.Id != id)
                    {
                        return new ApiResponse<DistrictDto>
                        {
                            Success = false,
                            MessageId = MessageId.E0000,
                            Message = "A district with this name already exists in this province"
                        };
                    }
                }

                // Update fields
                if (!string.IsNullOrEmpty(updateDistrictDto.Name))
                    district.Name = updateDistrictDto.Name;
                
                if (!string.IsNullOrEmpty(updateDistrictDto.Description))
                    district.Description = updateDistrictDto.Description;
                
                if (updateDistrictDto.ProvinceId != district.ProvinceId)
                    district.ProvinceId = updateDistrictDto.ProvinceId;

                district.UpdatedAt = DateTime.UtcNow;

                await _districtRepository.UpdateAsync(district);
                await _districtRepository.SaveChangesAsync();

                var districtDto = new DistrictDto
                {
                    Id = district.Id,
                    Name = district.Name,
                    Description = district.Description,
                    ProvinceId = district.ProvinceId,
                    ProvinceName = district.Province?.Name ?? "",
                    CreatedAt = district.CreatedAt,
                    UpdatedAt = district.UpdatedAt,
                    IsDeleted = district.IsDeleted
                };

                return new ApiResponse<DistrictDto>
                {
                    Data = districtDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<DistrictDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            try
            {
                var district = await _districtRepository.GetByIdAsync(id);
                if (district == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "District not found"
                    };
                }

                await _districtRepository.DeleteAsync(district);
                await _districtRepository.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = "District deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<bool>> ExistsAsync(Guid id)
        {
            try
            {
                var district = await _districtRepository.GetByIdAsync(id);
                var exists = district != null && !district.IsDeleted;
                
                return new ApiResponse<bool>
                {
                    Data = exists,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
} 