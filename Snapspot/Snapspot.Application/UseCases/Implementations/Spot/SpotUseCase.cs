using Snapspot.Application.Models.Responses.Spot;
using Snapspot.Application.Models.Spots;
using Snapspot.Application.Models.Styles;
using Snapspot.Application.Repositories;
using Snapspot.Application.UseCases.Interfaces.Spot;
using Snapspot.Shared.Common;
using Snapspot.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpotEntity = Snapspot.Domain.Entities.Spot;

namespace Snapspot.Application.UseCases.Implementations.Spot
{
    public class SpotUseCase : ISpotUseCase
    {
        private readonly ISpotRepository _spotRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IProvinceRepository _provinceRepository;
        private readonly IStyleRepository _styleRepository;

        public SpotUseCase(
            ISpotRepository spotRepository,
            IDistrictRepository districtRepository,
            IProvinceRepository provinceRepository,
            IStyleRepository styleRepository)
        {
            _spotRepository = spotRepository;
            _districtRepository = districtRepository;
            _provinceRepository = provinceRepository;
            _styleRepository = styleRepository;
        }

        public async Task<ApiResponse<SpotDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var spot = await _spotRepository.GetByIdAsync(id);
                if (spot == null)
                {
                    return new ApiResponse<SpotDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Spot not found"
                    };
                }

                
                var styles = await _spotRepository.GetStylesBySpotIdAsync(id);

                var spotDto = new SpotDto
                {
                    Id = spot.Id,
                    Name = spot.Name,
                    Description = spot.Description,
                    Latitude = spot.Latitude,
                    Longitude = spot.Longitude,
                    DistrictId = spot.DistrictId,
                    DistrictName = spot.District?.Name ?? "",
                    ProvinceName = spot.District?.Province?.Name ?? "",
                    Address = spot.Address,
                    ImageUrl = spot.ImageUrl,
                    Time = spot.Time,
                    CreatedAt = spot.CreatedAt,
                    UpdatedAt = spot.UpdatedAt,
                    IsDeleted = spot.IsDeleted,
                    Styles = styles.ToList() 
                };

                return new ApiResponse<SpotDto>
                {
                    Data = spotDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<SpotDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<SpotDto>>> GetAllAsync()
        {
            try
            {
                var spots = await _spotRepository.GetAllAsync();

                var spotDtos = new List<SpotDto>();

                foreach (var spot in spots)
                {
                    var styles = await _spotRepository.GetStylesBySpotIdAsync(spot.Id);

                    var spotDto = new SpotDto
                    {
                        Id = spot.Id,
                        Name = spot.Name,
                        Description = spot.Description,
                        Latitude = spot.Latitude,
                        Longitude = spot.Longitude,
                        DistrictId = spot.DistrictId,
                        DistrictName = spot.District?.Name ?? "",
                        ProvinceName = spot.District?.Province?.Name ?? "",
                        Address = spot.Address,
                        ImageUrl = spot.ImageUrl,
                        Time = spot.Time,
                        CreatedAt = spot.CreatedAt,
                        UpdatedAt = spot.UpdatedAt,
                        IsDeleted = spot.IsDeleted,
                        Styles = styles.ToList() // Thêm .ToList() để convert từ IEnumerable sang List
                    };

                    spotDtos.Add(spotDto);
                }

                return new ApiResponse<IEnumerable<SpotDto>>
                {
                    Data = spotDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<SpotDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<SpotDto>> CreateAsync(CreateSpotDto createSpotDto)
        {
            try
            {
                // Business logic: Validate district exists
                var district = await _districtRepository.GetByIdAsync(createSpotDto.DistrictId);
                if (district == null)
                {
                    return new ApiResponse<SpotDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "District not found"
                    };
                }

                // Business logic: Check if spot name already exists in the same district
                var allSpots = await _spotRepository.GetAllAsync();
                var existingSpot = allSpots.FirstOrDefault(x => x.Name == createSpotDto.Name && x.DistrictId == createSpotDto.DistrictId);
                if (existingSpot != null)
                {
                    return new ApiResponse<SpotDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "A spot with this name already exists in this district"
                    };
                }

                var spot = new SpotEntity
                {
                    Id = Guid.NewGuid(),
                    Name = createSpotDto.Name,
                    Description = createSpotDto.Description,
                    Latitude = createSpotDto.Latitude,
                    Longitude = createSpotDto.Longitude,
                    DistrictId = createSpotDto.DistrictId,
                    Address = createSpotDto.Address,
                    ImageUrl = createSpotDto.ImageUrl,
                    Time = createSpotDto.Time,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _spotRepository.AddAsync(spot);
                await _spotRepository.SaveChangesAsync();

                var spotDto = new SpotDto
                {
                    Id = spot.Id,
                    Name = spot.Name,
                    Description = spot.Description,
                    Latitude = spot.Latitude,
                    Longitude = spot.Longitude,
                    DistrictId = spot.DistrictId,
                    DistrictName = district.Name,
                    ProvinceName = district.Province?.Name ?? "",
                    Address = spot.Address,
                    ImageUrl = spot.ImageUrl,
                    Time = spot.Time,
                    CreatedAt = spot.CreatedAt,
                    UpdatedAt = spot.UpdatedAt,
                    IsDeleted = spot.IsDeleted
                };

                return new ApiResponse<SpotDto>
                {
                    Data = spotDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<SpotDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<SpotDto>> UpdateAsync(Guid id, UpdateSpotDto updateSpotDto)
        {
            try
            {
                var spot = await _spotRepository.GetByIdAsync(id);
                if (spot == null)
                {
                    return new ApiResponse<SpotDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Spot not found"
                    };
                }

                // Business logic: Validate district exists if changing
                if (updateSpotDto.DistrictId != spot.DistrictId)
                {
                    var district = await _districtRepository.GetByIdAsync(updateSpotDto.DistrictId);
                    if (district == null)
                    {
                        return new ApiResponse<SpotDto>
                        {
                            Success = false,
                            MessageId = MessageId.E0000,
                            Message = "District not found"
                        };
                    }
                }

                // Business logic: Check if spot name already exists in the same district
                if (!string.IsNullOrEmpty(updateSpotDto.Name) && updateSpotDto.Name != spot.Name)
                {
                    var districtId = updateSpotDto.DistrictId;
                    var allSpots = await _spotRepository.GetAllAsync();
                    var existingSpot = allSpots.FirstOrDefault(x => x.Name == updateSpotDto.Name && x.DistrictId == districtId);
                    if (existingSpot != null && existingSpot.Id != id)
                    {
                        return new ApiResponse<SpotDto>
                        {
                            Success = false,
                            MessageId = MessageId.E0000,
                            Message = "A spot with this name already exists in this district"
                        };
                    }
                }

                // Update fields
                if (!string.IsNullOrEmpty(updateSpotDto.Name))
                    spot.Name = updateSpotDto.Name;
                
                if (!string.IsNullOrEmpty(updateSpotDto.Description))
                    spot.Description = updateSpotDto.Description;
                
                if (updateSpotDto.Latitude != spot.Latitude)
                    spot.Latitude = updateSpotDto.Latitude;
                
                if (updateSpotDto.Longitude != spot.Longitude)
                    spot.Longitude = updateSpotDto.Longitude;
                
                if (updateSpotDto.DistrictId != spot.DistrictId)
                    spot.DistrictId = updateSpotDto.DistrictId;
                
                if (!string.IsNullOrEmpty(updateSpotDto.Address))
                    spot.Address = updateSpotDto.Address;
                
                if (!string.IsNullOrEmpty(updateSpotDto.ImageUrl))
                    spot.ImageUrl = updateSpotDto.ImageUrl;

                spot.UpdatedAt = DateTime.UtcNow;

                await _spotRepository.UpdateAsync(spot);
                await _spotRepository.SaveChangesAsync();

                var spotDto = new SpotDto
                {
                    Id = spot.Id,
                    Name = spot.Name,
                    Description = spot.Description,
                    Latitude = spot.Latitude,
                    Longitude = spot.Longitude,
                    DistrictId = spot.DistrictId,
                    DistrictName = spot.District?.Name ?? "",
                    ProvinceName = spot.District?.Province?.Name ?? "",
                    Address = spot.Address,
                    ImageUrl = spot.ImageUrl,
                    CreatedAt = spot.CreatedAt,
                    UpdatedAt = spot.UpdatedAt,
                    IsDeleted = spot.IsDeleted
                };

                return new ApiResponse<SpotDto>
                {
                    Data = spotDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<SpotDto>
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
                var spot = await _spotRepository.GetByIdAsync(id);
                if (spot == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Spot not found"
                    };
                }

                await _spotRepository.DeleteAsync(spot);
                await _spotRepository.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = "Spot deleted successfully"
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

        public async Task<ApiResponse<IEnumerable<SpotDto>>> GetByDistrictIdAsync(Guid districtId)
        {
            try
            {
                var spots = await _spotRepository.GetByDistrictIdAsync(districtId);
                
                var spotDtos = spots.Select(s => new SpotDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    DistrictId = s.DistrictId,
                    DistrictName = s.District?.Name ?? "",
                    ProvinceName = s.District?.Province?.Name ?? "",
                    Address = s.Address,
                    ImageUrl = s.ImageUrl,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    IsDeleted = s.IsDeleted
                }).ToList();

                return new ApiResponse<IEnumerable<SpotDto>>
                {
                    Data = spotDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<SpotDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<SpotDto>>> GetByProvinceIdAsync(Guid provinceId)
        {
            try
            {
                var allSpots = await _spotRepository.GetAllAsync();
                var spots = allSpots.Where(x => x.District != null && x.District.ProvinceId == provinceId).ToList();
                
                var spotDtos = spots.Select(s => new SpotDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    DistrictId = s.DistrictId,
                    DistrictName = s.District?.Name ?? "",
                    ProvinceName = s.District?.Province?.Name ?? "",
                    Address = s.Address,
                    ImageUrl = s.ImageUrl,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    IsDeleted = s.IsDeleted
                }).ToList();

                return new ApiResponse<IEnumerable<SpotDto>>
                {
                    Data = spotDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<SpotDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<SpotDto>>> SearchSpotsAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return new ApiResponse<IEnumerable<SpotDto>>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Search term cannot be empty"
                    };
                }

                // Since SearchAsync method doesn't exist, we'll implement a simple search
                var spots = await _spotRepository.GetAllAsync();
                var filteredSpots = spots.Where(s => 
                    s.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    s.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    s.Address.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                );
                
                var spotDtos = filteredSpots.Select(s => new SpotDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    DistrictId = s.DistrictId,
                    DistrictName = s.District?.Name ?? "",
                    ProvinceName = s.District?.Province?.Name ?? "",
                    Address = s.Address,
                    ImageUrl = s.ImageUrl,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    IsDeleted = s.IsDeleted
                }).ToList();

                return new ApiResponse<IEnumerable<SpotDto>>
                {
                    Data = spotDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<SpotDto>>
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
                var spot = await _spotRepository.GetByIdAsync(id);
                var exists = spot != null && !spot.IsDeleted;
                
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

        public async Task<ApiResponse<IEnumerable<GetAllSpotWithDistanceReponse>>> GetAllWithDistanceAsync(double ulat, double ulon)
        {
            try
            {
                var spots = await _spotRepository.GetAllAsync();

                var result = spots
                    .Where(s => s.Latitude.HasValue && s.Longitude.HasValue)
                    .Select(s => new GetAllSpotWithDistanceReponse
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        Latitude = s.Latitude,
                        Longitude = s.Longitude,
                        DistrictId = s.DistrictId,
                        DistrictName = s.District?.Name ?? "",
                        ProvinceName = s.District?.Province?.Name ?? "",
                        Address = s.Address,
                        ImageUrl = s.ImageUrl,
                        CreatedAt = s.CreatedAt ,
                        UpdatedAt = s.UpdatedAt,
                        IsDeleted = s.IsDeleted,
                        Distance = (decimal?)Haversine(ulat, ulon, s.Latitude.Value, s.Longitude.Value)
                    })
                    .OrderBy(s => s.Distance)
                    .ToList();

                return new ApiResponse<IEnumerable<GetAllSpotWithDistanceReponse>>
                {
                    Data = result,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<GetAllSpotWithDistanceReponse>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        private static double Haversine(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // km
            double dLat = DegreesToRadians(lat2 - lat1);
            double dLon = DegreesToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var km = R * c;
            return km + km*10/100;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
} 