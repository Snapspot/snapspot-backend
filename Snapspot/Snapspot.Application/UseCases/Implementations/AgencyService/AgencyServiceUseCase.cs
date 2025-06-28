using Snapspot.Application.Models.AgencyServices;
using Snapspot.Application.Repositories;
using Snapspot.Application.UseCases.Interfaces.AgencyService;
using Snapspot.Domain.Entities;
using Snapspot.Shared.Common;
using Snapspot.Shared.Constants;

namespace Snapspot.Application.UseCases.Implementations.AgencyService
{
    public class AgencyServiceUseCase : IAgencyServiceUseCase
    {
        private readonly IAgencyServiceRepository _agencyServiceRepository;
        private readonly IAgencyRepository _agencyRepository;

        public AgencyServiceUseCase(IAgencyServiceRepository agencyServiceRepository, IAgencyRepository agencyRepository)
        {
            _agencyServiceRepository = agencyServiceRepository;
            _agencyRepository = agencyRepository;
        }

        public async Task<ApiResponse<IEnumerable<AgencyServiceDto>>> GetAllAsync()
        {
            try
            {
                var agencyServices = await _agencyServiceRepository.GetAllAsync();
                var agencyServiceDtos = agencyServices.Select(service => new AgencyServiceDto
                {
                    Id = service.Id,
                    Name = service.Name,
                    Color = service.Color,
                    CreatedAt = service.CreatedAt,
                    UpdatedAt = service.UpdatedAt
                });

                return ApiResponse<IEnumerable<AgencyServiceDto>>.Ok(agencyServiceDtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<AgencyServiceDto>>.Fail("E0001");
            }
        }

        public async Task<ApiResponse<AgencyServiceDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var agencyService = await _agencyServiceRepository.GetByIdAsync(id);
                if (agencyService == null)
                    return ApiResponse<AgencyServiceDto>.Fail("E0002");

                var agencyServiceDto = new AgencyServiceDto
                {
                    Id = agencyService.Id,
                    Name = agencyService.Name,
                    Color = agencyService.Color,
                    CreatedAt = agencyService.CreatedAt,
                    UpdatedAt = agencyService.UpdatedAt
                };

                return ApiResponse<AgencyServiceDto>.Ok(agencyServiceDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<AgencyServiceDto>.Fail("E0001");
            }
        }

        public async Task<ApiResponse<IEnumerable<AgencyServiceDto>>> GetByAgencyIdAsync(Guid agencyId)
        {
            try
            {
                var agencyServices = await _agencyServiceRepository.GetAllAsync();
                var agencyServiceDtos = agencyServices
                    .Where(service => service.Agencies.Any(agency => agency.Id == agencyId))
                    .Select(service => new AgencyServiceDto
                    {
                        Id = service.Id,
                        Name = service.Name,
                        Color = service.Color,
                        CreatedAt = service.CreatedAt,
                        UpdatedAt = service.UpdatedAt
                    });

                return ApiResponse<IEnumerable<AgencyServiceDto>>.Ok(agencyServiceDtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<AgencyServiceDto>>.Fail("E0001");
            }
        }

        public async Task<ApiResponse<AgencyServiceDto>> CreateAsync(CreateAgencyServiceDto createAgencyServiceDto)
        {
            try
            {
                var agencyService = new Domain.Entities.AgencyService
                {
                    Id = Guid.NewGuid(),
                    Name = createAgencyServiceDto.Name,
                    Color = createAgencyServiceDto.Color,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _agencyServiceRepository.AddAsync(agencyService);

                var agencyServiceDto = new AgencyServiceDto
                {
                    Id = agencyService.Id,
                    Name = agencyService.Name,
                    Color = agencyService.Color,
                    CreatedAt = agencyService.CreatedAt,
                    UpdatedAt = agencyService.UpdatedAt
                };

                return ApiResponse<AgencyServiceDto>.Ok(agencyServiceDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<AgencyServiceDto>.Fail("E0001");
            }
        }

        public async Task<ApiResponse<AgencyServiceDto>> UpdateAsync(Guid id, UpdateAgencyServiceDto updateAgencyServiceDto)
        {
            try
            {
                var agencyService = await _agencyServiceRepository.GetByIdAsync(id);
                if (agencyService == null)
                    return ApiResponse<AgencyServiceDto>.Fail("E0002");

                agencyService.Name = updateAgencyServiceDto.Name;
                agencyService.Color = updateAgencyServiceDto.Color;
                agencyService.UpdatedAt = DateTime.UtcNow;

                await _agencyServiceRepository.UpdateAsync(agencyService);

                var agencyServiceDto = new AgencyServiceDto
                {
                    Id = agencyService.Id,
                    Name = agencyService.Name,
                    Color = agencyService.Color,
                    CreatedAt = agencyService.CreatedAt,
                    UpdatedAt = agencyService.UpdatedAt
                };

                return ApiResponse<AgencyServiceDto>.Ok(agencyServiceDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<AgencyServiceDto>.Fail("E0001");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var agencyService = await _agencyServiceRepository.GetByIdAsync(id);
                if (agencyService == null)
                    return ApiResponse<bool>.Fail("E0002");

                await _agencyServiceRepository.DeleteAsync(agencyService);
                return ApiResponse<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail("E0001");
            }
        }

        public async Task<ApiResponse<bool>> AddToAgencyAsync(Guid id, Guid agencyId)
        {
            try
            {
                var agencyService = await _agencyServiceRepository.GetByIdAsync(id);
                if (agencyService == null)
                    return ApiResponse<bool>.Fail("E0002");

                var agency = await _agencyRepository.GetByIdAsync(agencyId);
                if (agency == null)
                    return ApiResponse<bool>.Fail("E0003");

                if (!agencyService.Agencies.Any(a => a.Id == agencyId))
                {
                    agencyService.Agencies.Add(agency);
                    await _agencyServiceRepository.UpdateAsync(agencyService);
                }

                return ApiResponse<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail("E0001");
            }
        }

        public async Task<ApiResponse<bool>> RemoveFromAgencyAsync(Guid id, Guid agencyId)
        {
            try
            {
                var agencyService = await _agencyServiceRepository.GetByIdAsync(id);
                if (agencyService == null)
                    return ApiResponse<bool>.Fail("E0002");

                var agency = agencyService.Agencies.FirstOrDefault(a => a.Id == agencyId);
                if (agency != null)
                {
                    agencyService.Agencies.Remove(agency);
                    await _agencyServiceRepository.UpdateAsync(agencyService);
                }

                return ApiResponse<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail("E0001");
            }
        }
    }
} 