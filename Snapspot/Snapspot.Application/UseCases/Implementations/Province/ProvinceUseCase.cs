using Snapspot.Application.Models.Provinces;
using Snapspot.Application.Repositories;
using Snapspot.Application.UseCases.Interfaces.Province;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Implementations.Province
{
    public class ProvinceUseCase : IProvinceUseCase
    {
        private readonly IProvinceRepository _provinceRepository;

        public ProvinceUseCase(IProvinceRepository provinceRepository)
        {
            _provinceRepository = provinceRepository;
        }

        public async Task<ApiResponse<ProvinceDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var province = await _provinceRepository.GetByIdAsync(id);
                if (province == null)
                {
                    return new ApiResponse<ProvinceDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Province not found"
                    };
                }

                var provinceDto = new ProvinceDto
                {
                    Id = province.Id,
                    Name = province.Name,
                    Description = province.Description,
                    CreatedAt = province.CreatedAt,
                    UpdatedAt = province.UpdatedAt,
                    IsDeleted = province.IsDeleted,
                    Districts = province.Districts?.Select(d => new Models.Districts.DistrictDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Description = d.Description,
                        ProvinceId = d.ProvinceId,
                        ProvinceName = d.Province?.Name ?? "",
                        CreatedAt = d.CreatedAt,
                        UpdatedAt = d.UpdatedAt,
                        IsDeleted = d.IsDeleted
                    }).ToList() ?? new List<Models.Districts.DistrictDto>()
                };

                return new ApiResponse<ProvinceDto>
                {
                    Data = provinceDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProvinceDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<ProvinceDto>>> GetAllAsync()
        {
            try
            {
                var provinces = await _provinceRepository.GetAllAsync();
                
                var provinceDtos = provinces.Select(p => new ProvinceDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    IsDeleted = p.IsDeleted,
                    Districts = p.Districts?.Select(d => new Models.Districts.DistrictDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Description = d.Description,
                        ProvinceId = d.ProvinceId,
                        ProvinceName = d.Province?.Name ?? "",
                        CreatedAt = d.CreatedAt,
                        UpdatedAt = d.UpdatedAt,
                        IsDeleted = d.IsDeleted
                    }).ToList() ?? new List<Models.Districts.DistrictDto>()
                }).ToList();

                return new ApiResponse<IEnumerable<ProvinceDto>>
                {
                    Data = provinceDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<ProvinceDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<ProvinceDto>> CreateAsync(CreateProvinceDto createProvinceDto)
        {
            try
            {
                var allProvinces = await _provinceRepository.GetAllAsync();
                var existingProvince = allProvinces.FirstOrDefault(x => x.Name == createProvinceDto.Name);
                if (existingProvince != null)
                {
                    return new ApiResponse<ProvinceDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "A province with this name already exists"
                    };
                }

                var province = new Domain.Entities.Province
                {
                    Id = Guid.NewGuid(),
                    Name = createProvinceDto.Name,
                    Description = createProvinceDto.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _provinceRepository.AddAsync(province);
                await _provinceRepository.SaveChangesAsync();

                var provinceDto = new ProvinceDto
                {
                    Id = province.Id,
                    Name = province.Name,
                    Description = province.Description,
                    CreatedAt = province.CreatedAt,
                    UpdatedAt = province.UpdatedAt,
                    IsDeleted = province.IsDeleted,
                    Districts = new List<Models.Districts.DistrictDto>()
                };

                return new ApiResponse<ProvinceDto>
                {
                    Data = provinceDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProvinceDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<ProvinceDto>> UpdateAsync(Guid id, UpdateProvinceDto updateProvinceDto)
        {
            try
            {
                var province = await _provinceRepository.GetByIdAsync(id);
                if (province == null)
                {
                    return new ApiResponse<ProvinceDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Province not found"
                    };
                }

                if (!string.IsNullOrEmpty(updateProvinceDto.Name) && updateProvinceDto.Name != province.Name)
                {
                    var allProvinces = await _provinceRepository.GetAllAsync();
                    var existingProvince = allProvinces.FirstOrDefault(x => x.Name == updateProvinceDto.Name);
                    if (existingProvince != null && existingProvince.Id != id)
                    {
                        return new ApiResponse<ProvinceDto>
                        {
                            Success = false,
                            MessageId = MessageId.E0000,
                            Message = "A province with this name already exists"
                        };
                    }
                }

                if (!string.IsNullOrEmpty(updateProvinceDto.Name))
                    province.Name = updateProvinceDto.Name;
                
                if (!string.IsNullOrEmpty(updateProvinceDto.Description))
                    province.Description = updateProvinceDto.Description;

                province.UpdatedAt = DateTime.UtcNow;

                await _provinceRepository.UpdateAsync(province);
                await _provinceRepository.SaveChangesAsync();

                var provinceDto = new ProvinceDto
                {
                    Id = province.Id,
                    Name = province.Name,
                    Description = province.Description,
                    CreatedAt = province.CreatedAt,
                    UpdatedAt = province.UpdatedAt,
                    IsDeleted = province.IsDeleted,
                    Districts = province.Districts?.Select(d => new Models.Districts.DistrictDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Description = d.Description,
                        ProvinceId = d.ProvinceId,
                        ProvinceName = d.Province?.Name ?? "",
                        CreatedAt = d.CreatedAt,
                        UpdatedAt = d.UpdatedAt,
                        IsDeleted = d.IsDeleted
                    }).ToList() ?? new List<Models.Districts.DistrictDto>()
                };

                return new ApiResponse<ProvinceDto>
                {
                    Data = provinceDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProvinceDto>
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
                var province = await _provinceRepository.GetByIdAsync(id);
                if (province == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Province not found"
                    };
                }

                await _provinceRepository.DeleteAsync(province);
                await _provinceRepository.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = "Province deleted successfully"
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
                var province = await _provinceRepository.GetByIdAsync(id);
                var exists = province != null && !province.IsDeleted;
                
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