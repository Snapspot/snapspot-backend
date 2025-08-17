using Snapspot.Application.Models.Agencies;
using Snapspot.Application.Models.Responses.Agency;
using Snapspot.Application.Models.Responses.ThirdParty;
using Snapspot.Application.Repositories;
using Snapspot.Application.UseCases.Interfaces.Agency;
using Snapspot.Domain.Entities;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Implementations.Agency
{
    public class AgencyUseCase : IAgencyUseCase
    {
        private readonly IAgencyRepository _agencyRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ISpotRepository _spotRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAgencyServiceRepository _agencyServiceRepository;
        private readonly IAgencyViewRepository _agencyViewRepository;

        public AgencyUseCase(
            IAgencyRepository agencyRepository,
            ICompanyRepository companyRepository,
            ISpotRepository spotRepository,
            IUserRepository userRepository,
            IAgencyServiceRepository agencyServiceRepository,
            IAgencyViewRepository agencyViewRepository)
        {
            _agencyRepository = agencyRepository;
            _companyRepository = companyRepository;
            _spotRepository = spotRepository;
            _userRepository = userRepository;
            _agencyServiceRepository = agencyServiceRepository;
            _agencyViewRepository = agencyViewRepository;
        }

        public async Task<ApiResponse<AgencyDto>> GetByIdAsync(Guid id, string? userId)
        {
            try
            {
                var agency = await _agencyRepository.GetByIdAsync(id);
                if (agency == null)
                {
                    return new ApiResponse<AgencyDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Agency not found"
                    };
                }

                if(userId != null)
                {
                    try
                    {
                        Guid userGuid = Guid.Parse(userId);
                       
                        bool isExist = await _agencyViewRepository.IsExist(agency.Id, userGuid);
                        if (!isExist)
                        {
                            var newItem = new AgencyView
                            {
                                UserId = userGuid,
                                AgencyId = agency.Id,
                                ViewDate = DateTime.Now,
                            };
                            await _agencyViewRepository.Create(newItem);
                        }                       
                    }
                    catch (FormatException)
                    {
                        
                    }
                }

                var agencyDto = new AgencyDto
                {
                    Id = agency.Id,
                    Name = agency.Name,
                    Address = agency.Address,
                    Fullname = agency.Fullname,
                    PhoneNumber = agency.PhoneNumber,
                    AvatarUrl = agency.AvatarUrl,
                    Rating = agency.Rating,
                    CompanyId = agency.CompanyId,
                    CompanyName = agency.Company?.Name ?? "",
                    SpotId = agency.SpotId,
                    SpotName = agency.Spot?.Name ?? "",
                    Description = agency.Description,
                    CreatedAt = agency.CreatedAt,
                    UpdatedAt = agency.UpdatedAt,
                    IsDeleted = agency.IsDeleted,
                    Services = agency.Services?.Select(s => new Models.AgencyServices.AgencyServiceDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Color = s.Color,
                        CreatedAt = s.CreatedAt,
                        UpdatedAt = s.UpdatedAt,
                        IsDeleted = s.IsDeleted
                    }).ToList() ?? new List<Models.AgencyServices.AgencyServiceDto>(),
                    Feedbacks = agency.Feedbacks?.Select(f => new FeedbackDto
                    {
                        Id = f.Id,
                        Content = f.Content,
                        Rating = f.Rating,
                        AgencyId = f.AgencyId,
                        UserId = f.UserId,
                        UserName = f.User?.Fullname ?? "",
                        CreatedAt = f.CreatedAt,
                        UpdatedAt = f.UpdatedAt,
                        IsDeleted = f.IsDeleted
                    }).ToList() ?? new List<FeedbackDto>()
                };

                return new ApiResponse<AgencyDto>
                {
                    Data = agencyDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AgencyDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<AgencyDto>>> GetAllAsync()
        {
            try
            {
                var agencies = await _agencyRepository.GetAllAsync();
                
                var agencyDtos = agencies.Select(a => new AgencyDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Address = a.Address,
                    Fullname = a.Fullname,
                    PhoneNumber = a.PhoneNumber,
                    AvatarUrl = a.AvatarUrl,
                    Rating = a.Rating,
                    CompanyId = a.CompanyId,
                    CompanyName = a.Company?.Name ?? "",
                    SpotId = a.SpotId,
                    SpotName = a.Spot?.Name ?? "",
                    Description = a.Description,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    IsDeleted = a.IsDeleted,
                    Services = a.Services?.Select(s => new Models.AgencyServices.AgencyServiceDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Color = s.Color,
                        CreatedAt = s.CreatedAt,
                        UpdatedAt = s.UpdatedAt,
                        IsDeleted = s.IsDeleted
                    }).ToList() ?? new List<Models.AgencyServices.AgencyServiceDto>(),
                    Feedbacks = a.Feedbacks?.Select(f => new FeedbackDto
                    {
                        Id = f.Id,
                        Content = f.Content,
                        Rating = f.Rating,
                        AgencyId = f.AgencyId,
                        UserId = f.UserId,
                        UserName = f.User?.Fullname ?? "",
                        CreatedAt = f.CreatedAt,
                        UpdatedAt = f.UpdatedAt,
                        IsDeleted = f.IsDeleted
                    }).ToList() ?? new List<FeedbackDto>()
                }).ToList();

                return new ApiResponse<IEnumerable<AgencyDto>>
                {
                    Data = agencyDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<AgencyDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<AgencyCreationResponse>> CreateAsync(CreateAgencyDto createAgencyDto, string? userId)
        {
            if (userId == null)
                return new ApiResponse<AgencyCreationResponse>
                {
                    Success = false,
                    MessageId = MessageId.E0010,
                    Message = Message.GetMessageById(MessageId.E0010)
                };
            var company = await GetCompanyByUser(userId);
            if (company == null)
            {
                return new ApiResponse<AgencyCreationResponse>
                {
                    Success = false,
                    MessageId = MessageId.E0020,
                    Message = Message.GetMessageById(MessageId.E0020)
                };
            }
            try
            {
                // Business logic: Validate spot exists
                var spot = await _spotRepository.GetByIdAsync(createAgencyDto.SpotId);
                if (spot == null)
                {
                    return new ApiResponse<AgencyCreationResponse>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Spot not found"
                    };
                }

                // Business logic: Check if agency name already exists in the same company and spot
                var allAgencies = await _agencyRepository.GetAllAsync();
                var existingAgency = allAgencies.FirstOrDefault(x => x.Name == createAgencyDto.Name && x.CompanyId == company.Id);
                if (existingAgency != null)
                {
                    return new ApiResponse<AgencyCreationResponse>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "An agency with this name already exists in this company"
                    };
                }

                List<Domain.Entities.AgencyService> agencyServices;

                var enumarabelAgencyServices = await _agencyServiceRepository
                    .FindAllAsync(createAgencyDto.AgencyServiceIds);
                agencyServices = enumarabelAgencyServices.ToList();

                var agency = new Domain.Entities.Agency
                {
                    Id = Guid.NewGuid(),
                    Name = createAgencyDto.Name,
                    Address = createAgencyDto.Address,
                    Fullname = createAgencyDto.Fullname,
                    PhoneNumber = createAgencyDto.PhoneNumber,
                    AvatarUrl = createAgencyDto.AvatarUrl,
                    CompanyId = company.Id,
                    SpotId = createAgencyDto.SpotId,
                    Description = createAgencyDto.Description,
                    Rating = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                    Services = agencyServices
                };

                await _agencyRepository.AddAsync(agency);
                await _agencyRepository.SaveChangesAsync();

                var agencyDto = new AgencyCreationResponse
                {
                    Id = agency.Id,
                    Name = agency.Name,
                    Address = agency.Address,
                    Fullname = agency.Fullname,
                    PhoneNumber = agency.PhoneNumber,
                    AvatarUrl = agency.AvatarUrl,
                    Rating = agency.Rating,
                    CompanyId = agency.CompanyId,
                    CompanyName = company.Name,
                    SpotId = agency.SpotId,
                    SpotName = spot.Name,
                    Description = agency.Description,
                    CreatedAt = agency.CreatedAt,
                    UpdatedAt = agency.UpdatedAt,
                    IsDeleted = agency.IsDeleted,
                    Services = agencyServices.Select(s => new AgencyServiceResponse
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Color = s.Color
                    }).ToList(),
                    Feedbacks = new List<FeedbackDto>()
                };

                return new ApiResponse<AgencyCreationResponse>
                {
                    Data = agencyDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AgencyCreationResponse>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        private async Task<Domain.Entities.Company?> GetCompanyByUser(string userIdString)
        {
            var userId = Guid.Parse(userIdString);
            var user = await _userRepository.GetByUserIdAsync(userId);
            if (user == null) return null;
            var company = await _companyRepository.GetByUserIdAsync(user.Id);
            return company;
        }

        public async Task<ApiResponse<AgencyDto>> UpdateAsync(Guid id, UpdateAgencyDto updateAgencyDto)
        {
            try
            {
                var agency = await _agencyRepository.GetByIdAsync(id);
                if (agency == null)
                {
                    return new ApiResponse<AgencyDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Agency not found"
                    };
                }

                // Business logic: Validate company exists if changing
                if (updateAgencyDto.CompanyId != agency.CompanyId)
                {
                    var company = await _companyRepository.GetByIdAsync(updateAgencyDto.CompanyId);
                    if (company == null)
                    {
                        return new ApiResponse<AgencyDto>
                        {
                            Success = false,
                            MessageId = MessageId.E0000,
                            Message = "Company not found"
                        };
                    }
                }

                // Business logic: Check if agency name already exists in the same company
                if (!string.IsNullOrEmpty(updateAgencyDto.Name) && updateAgencyDto.Name != agency.Name)
                {
                    var companyId = updateAgencyDto.CompanyId;
                    var allAgencies = await _agencyRepository.GetAllAsync();
                    var existingAgency = allAgencies.FirstOrDefault(x => x.Name == updateAgencyDto.Name && x.CompanyId == companyId);
                    if (existingAgency != null && existingAgency.Id != id)
                    {
                        return new ApiResponse<AgencyDto>
                        {
                            Success = false,
                            MessageId = MessageId.E0000,
                            Message = "An agency with this name already exists in this company"
                        };
                    }
                }

                // Update fields
                if (!string.IsNullOrEmpty(updateAgencyDto.Name))
                    agency.Name = updateAgencyDto.Name;
                
                if (!string.IsNullOrEmpty(updateAgencyDto.Address))
                    agency.Address = updateAgencyDto.Address;
                
                if (!string.IsNullOrEmpty(updateAgencyDto.Fullname))
                    agency.Fullname = updateAgencyDto.Fullname;
                
                if (!string.IsNullOrEmpty(updateAgencyDto.PhoneNumber))
                    agency.PhoneNumber = updateAgencyDto.PhoneNumber;
                
                if (!string.IsNullOrEmpty(updateAgencyDto.AvatarUrl))
                    agency.AvatarUrl = updateAgencyDto.AvatarUrl;              
                
                if (updateAgencyDto.SpotId != null)
                    agency.SpotId = updateAgencyDto.SpotId;
                
                if (!string.IsNullOrEmpty(updateAgencyDto.Description))
                    agency.Description = updateAgencyDto.Description;

                agency.UpdatedAt = DateTime.UtcNow;

                await _agencyRepository.UpdateAsync(agency);
                await _agencyRepository.SaveChangesAsync();

                var agencyDto = new AgencyDto
                {
                    Id = agency.Id,
                    Name = agency.Name,
                    Address = agency.Address,
                    Fullname = agency.Fullname,
                    PhoneNumber = agency.PhoneNumber,
                    AvatarUrl = agency.AvatarUrl,
                    Rating = agency.Rating,
                    CompanyId = agency.CompanyId,
                    CompanyName = agency.Company?.Name ?? "",
                    SpotId = agency.SpotId,
                    SpotName = agency.Spot?.Name ?? "",
                    Description = agency.Description,
                    CreatedAt = agency.CreatedAt,
                    UpdatedAt = agency.UpdatedAt,
                    IsDeleted = agency.IsDeleted,
                    Services = agency.Services?.Select(s => new Models.AgencyServices.AgencyServiceDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Color = s.Color,
                        CreatedAt = s.CreatedAt,
                        UpdatedAt = s.UpdatedAt,
                        IsDeleted = s.IsDeleted
                    }).ToList() ?? new List<Models.AgencyServices.AgencyServiceDto>(),
                    Feedbacks = agency.Feedbacks?.Select(f => new FeedbackDto
                    {
                        Id = f.Id,
                        Content = f.Content,
                        Rating = f.Rating,
                        AgencyId = f.AgencyId,
                        UserId = f.UserId,
                        UserName = f.User?.Fullname ?? "",
                        CreatedAt = f.CreatedAt,
                        UpdatedAt = f.UpdatedAt,
                        IsDeleted = f.IsDeleted
                    }).ToList() ?? new List<FeedbackDto>()
                };

                return new ApiResponse<AgencyDto>
                {
                    Data = agencyDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AgencyDto>
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
                var agency = await _agencyRepository.GetByIdAsync(id);
                if (agency == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Agency not found"
                    };
                }

                agency.IsDeleted = true;
                await _agencyRepository.UpdateAsync(agency);
                await _agencyRepository.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = "Agency deleted successfully"
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

        public async Task<ApiResponse<IEnumerable<AgencyDto>>> GetByCompanyIdAsync(Guid companyId)
        {
            try
            {
                var agencies = await _agencyRepository.GetByCompanyIdAsync(companyId);
                
                var agencyDtos = agencies.Select(a => new AgencyDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Address = a.Address,
                    Fullname = a.Fullname,
                    PhoneNumber = a.PhoneNumber,
                    AvatarUrl = a.AvatarUrl,
                    Rating = a.Rating,
                    CompanyId = a.CompanyId,
                    CompanyName = a.Company?.Name ?? "",
                    SpotId = a.SpotId,
                    SpotName = a.Spot?.Name ?? "",
                    Description = a.Description,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    IsDeleted = a.IsDeleted,
                    Services = a.Services?.Select(s => new Models.AgencyServices.AgencyServiceDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Color = s.Color,
                        CreatedAt = s.CreatedAt,
                        UpdatedAt = s.UpdatedAt,
                        IsDeleted = s.IsDeleted
                    }).ToList() ?? new List<Models.AgencyServices.AgencyServiceDto>(),
                    Feedbacks = a.Feedbacks?.Select(f => new FeedbackDto
                    {
                        Id = f.Id,
                        Content = f.Content,
                        Rating = f.Rating,
                        AgencyId = f.AgencyId,
                        UserId = f.UserId,
                        UserName = f.User?.Fullname ?? "",
                        CreatedAt = f.CreatedAt,
                        UpdatedAt = f.UpdatedAt,
                        IsDeleted = f.IsDeleted
                    }).ToList() ?? new List<FeedbackDto>()
                }).ToList();

                return new ApiResponse<IEnumerable<AgencyDto>>
                {
                    Data = agencyDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<AgencyDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<AgencyDto>>> GetBySpotIdAsync(Guid spotId)
        {
            try
            {
                var agencies = await _agencyRepository.GetBySpotIdAsync(spotId);
                
                var agencyDtos = agencies.Select(a => new AgencyDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Address = a.Address,
                    Fullname = a.Fullname,
                    PhoneNumber = a.PhoneNumber,
                    AvatarUrl = a.AvatarUrl,
                    Rating = a.Rating,
                    CompanyId = a.CompanyId,
                    CompanyName = a.Company?.Name ?? "",
                    SpotId = a.SpotId,
                    SpotName = a.Spot?.Name ?? "",
                    Description = a.Description,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    IsDeleted = a.IsDeleted,
                    Services = a.Services?.Select(s => new Models.AgencyServices.AgencyServiceDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Color = s.Color,
                        CreatedAt = s.CreatedAt,
                        UpdatedAt = s.UpdatedAt,
                        IsDeleted = s.IsDeleted
                    }).ToList() ?? new List<Models.AgencyServices.AgencyServiceDto>(),
                    Feedbacks = a.Feedbacks?.Select(f => new FeedbackDto
                    {
                        Id = f.Id,
                        Content = f.Content,
                        Rating = f.Rating,
                        AgencyId = f.AgencyId,
                        UserId = f.UserId,
                        UserName = f.User?.Fullname ?? "",
                        CreatedAt = f.CreatedAt,
                        UpdatedAt = f.UpdatedAt,
                        IsDeleted = f.IsDeleted
                    }).ToList() ?? new List<FeedbackDto>()
                }).ToList();

                return new ApiResponse<IEnumerable<AgencyDto>>
                {
                    Data = agencyDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<AgencyDto>>
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
                var agency = await _agencyRepository.GetByIdAsync(id);
                var exists = agency != null && !agency.IsDeleted;
                
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

        public async Task<ApiResponse<IEnumerable<AgencyDto>>> SearchAgenciesAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return new ApiResponse<IEnumerable<AgencyDto>>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Search term cannot be empty"
                    };
                }

                // Since SearchAsync method doesn't exist, we'll implement a simple search
                var agencies = await _agencyRepository.GetAllAsync();
                var filteredAgencies = agencies.Where(a => 
                    a.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    a.Fullname.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    a.Address.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                );
                
                var agencyDtos = filteredAgencies.Select(a => new AgencyDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Address = a.Address,
                    Fullname = a.Fullname,
                    PhoneNumber = a.PhoneNumber,
                    AvatarUrl = a.AvatarUrl,
                    Rating = a.Rating,
                    CompanyId = a.CompanyId,
                    CompanyName = a.Company?.Name ?? "",
                    SpotId = a.SpotId,
                    SpotName = a.Spot?.Name ?? "",
                    Description = a.Description,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    IsDeleted = a.IsDeleted,
                    Services = a.Services?.Select(s => new Models.AgencyServices.AgencyServiceDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Color = s.Color,
                        CreatedAt = s.CreatedAt,
                        UpdatedAt = s.UpdatedAt,
                        IsDeleted = s.IsDeleted
                    }).ToList() ?? new List<Models.AgencyServices.AgencyServiceDto>(),
                    Feedbacks = a.Feedbacks?.Select(f => new FeedbackDto
                    {
                        Id = f.Id,
                        Content = f.Content,
                        Rating = f.Rating,
                        AgencyId = f.AgencyId,
                        UserId = f.UserId,
                        UserName = f.User?.Fullname ?? "",
                        CreatedAt = f.CreatedAt,
                        UpdatedAt = f.UpdatedAt,
                        IsDeleted = f.IsDeleted
                    }).ToList() ?? new List<FeedbackDto>()
                }).ToList();

                return new ApiResponse<IEnumerable<AgencyDto>>
                {
                    Data = agencyDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<AgencyDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
} 