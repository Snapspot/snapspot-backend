using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Models.Agencies;
using Snapspot.Application.Models.Responses.ThirdParty;
using Snapspot.Application.Repositories;
using Snapspot.Application.UseCases.Interfaces.ThirdParty;
using Snapspot.Domain.Entities;
using Snapspot.Shared.Common;

namespace Snapspot.Application.UseCases.Implementations.ThirdParty
{
    public class ThirdPartyUseCase : IThirdPartyUseCase
    {
        private readonly IAgencyRepository _agencyRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ISellerPackageRepository _sellerPackageRepository;
        private readonly IFeedbackRepository _feedbackRepository;

        public ThirdPartyUseCase(IAgencyRepository agencyRepository, IUserRepository userRepository, ICompanyRepository companyRepository, ISellerPackageRepository sellerPackageRepository, IFeedbackRepository feedbackRepository)
        {
            _agencyRepository = agencyRepository;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _sellerPackageRepository = sellerPackageRepository;
            _feedbackRepository = feedbackRepository;
        }

        public async Task<ApiResponse<List<GetThirdPartyAgencyResponse>>> GetAgencies(string? userId)
        {
            if(userId == null)
                return new ApiResponse<List<GetThirdPartyAgencyResponse>>
                {
                    Success = false,
                    MessageId = MessageId.E0010,
                    Message = Message.GetMessageById(MessageId.E0010)
                };

            var company = await GetCompanyByUser(userId);
            if(company == null)
            {
                return new ApiResponse<List<GetThirdPartyAgencyResponse>>
                {
                    Success = false,
                    MessageId = MessageId.E0020,
                    Message = Message.GetMessageById(MessageId.E0020)
                };
            }

            try
            {
                var agencies = await _agencyRepository.GetByCompanyIdAsync(company.Id);

                var agencyDtos = agencies.Select(a => new GetThirdPartyAgencyResponse
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

                return new ApiResponse<List<GetThirdPartyAgencyResponse>>
                {
                    Data = agencyDtos.ToList(),
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<GetThirdPartyAgencyResponse>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        //TODO
        public async Task<ApiResponse<GetSerllerPackageInfor>> GetSellerPackageInfo(string? userId)
        {
            if (userId == null)
                return new ApiResponse<GetSerllerPackageInfor>
                {
                    Success = false,
                    MessageId = MessageId.E0010,
                    Message = Message.GetMessageById(MessageId.E0010)
                };

            var company = await GetCompanyByUser(userId);
            if (company == null)
            {
                return new ApiResponse<GetSerllerPackageInfor>
                {
                    Success = false,
                    MessageId = MessageId.E0020,
                    Message = Message.GetMessageById(MessageId.E0020)
                };
            }

            var info = new GetSerllerPackageInfor()
            {
                PackageName = "API chưa code xong",
                PackageImageUrl = "https://th.bing.com/th/id/OIP.NcAupDjIQDZzrnMh0yY2bAHaHT?o=7rm=3&rs=1&pid=ImgDetMain&o=7&rm=3",
                CurrentAgency = 1,
                TotalAgency = 1,
                RemainingDay = 30
            };

            return new ApiResponse<GetSerllerPackageInfor>
            {
                Data = info,
                Success = true,
                MessageId = MessageId.I0000,
                Message = Message.GetMessageById(MessageId.I0000)
            };
        }

        public async Task<ApiResponse<List<GetUserFeedback>>> GetUserFeedback(string? userId)
        {
            if (userId == null)
                return new ApiResponse<List<GetUserFeedback>>
                {
                    Success = false,
                    MessageId = MessageId.E0010,
                    Message = Message.GetMessageById(MessageId.E0010)
                };

            var company = await GetCompanyByUser(userId);
            if (company == null)
            {
                return new ApiResponse<List<GetUserFeedback>>
                {
                    Success = false,
                    MessageId = MessageId.E0020,
                    Message = Message.GetMessageById(MessageId.E0020)
                };
            }

            var feedbacks = await _feedbackRepository.GetFeedbackByCompanyId(company.Id);
            List<GetUserFeedback> result = feedbacks.Select(f => new GetUserFeedback
            {
                FullName = f.User.Fullname,
                AgencyName = f.Agency.Name,
                Rating = f.Rating,
                Content = f.Content,
                CreatedDate = f.CreatedAt
            }).ToList();

            return new ApiResponse<List<GetUserFeedback>>
            {
                Data = result,
                Success = true,
                MessageId = MessageId.E0020,
                Message = Message.GetMessageById(MessageId.E0020)
            };

        }

        private async Task<Domain.Entities.Company?> GetCompanyByUser(string userIdString)
        {
            var userId = Guid.Parse(userIdString);
            var user = await _userRepository.GetByUserIdAsync(userId);
            if(user == null) return null;
            var company = await _companyRepository.GetByUserIdAsync(user.Id);
            return company;
        }
    }
}
