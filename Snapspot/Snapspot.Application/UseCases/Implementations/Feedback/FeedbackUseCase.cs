using Snapspot.Application.Models.Agencies;
using Snapspot.Application.Repositories;
using Snapspot.Application.UseCases.Interfaces.Feedback;
using Snapspot.Shared.Common;
using Snapspot.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedbackEntity = Snapspot.Domain.Entities.Feedback;

namespace Snapspot.Application.UseCases.Implementations.Feedback
{
    public class FeedbackUseCase : IFeedbackUseCase
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IAgencyRepository _agencyRepository;
        private readonly IUserRepository _userRepository;

        public FeedbackUseCase(
            IFeedbackRepository feedbackRepository,
            IAgencyRepository agencyRepository,
            IUserRepository userRepository)
        {
            _feedbackRepository = feedbackRepository;
            _agencyRepository = agencyRepository;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<FeedbackDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var feedback = await _feedbackRepository.GetByIdAsync(id);
                if (feedback == null)
                {
                    return new ApiResponse<FeedbackDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Feedback not found"
                    };
                }

                var feedbackDto = new FeedbackDto
                {
                    Id = feedback.Id,
                    Content = feedback.Content,
                    Rating = feedback.Rating,
                    CreatedAt = feedback.CreatedAt,
                    UpdatedAt = feedback.UpdatedAt,
                    IsDeleted = feedback.IsDeleted,
                    AgencyId = feedback.AgencyId,
                    UserId = feedback.UserId,
                    UserName = feedback.User?.Fullname ?? ""
                };

                return new ApiResponse<FeedbackDto>
                {
                    Data = feedbackDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<FeedbackDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<FeedbackDto>>> GetAllAsync()
        {
            try
            {
                var feedbacks = await _feedbackRepository.GetAllAsync();
                
                var feedbackDtos = feedbacks.Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    Content = f.Content,
                    Rating = f.Rating,
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt,
                    IsDeleted = f.IsDeleted,
                    AgencyId = f.AgencyId,
                    UserId = f.UserId,
                    UserName = f.User?.Fullname ?? ""
                }).ToList();

                return new ApiResponse<IEnumerable<FeedbackDto>>
                {
                    Data = feedbackDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<FeedbackDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<PagingResponse<FeedbackDto>>> GetPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                var pagedFeedbacks = await _feedbackRepository.GetPagedAsync(pageNumber, pageSize);
                
                var feedbackDtos = pagedFeedbacks.Items.Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    Content = f.Content,
                    Rating = f.Rating,
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt,
                    IsDeleted = f.IsDeleted,
                    AgencyId = f.AgencyId,
                    UserId = f.UserId,
                    UserName = f.User?.Fullname ?? ""
                }).ToList();

                var pagedResponse = new PagingResponse<FeedbackDto>
                {
                    Items = feedbackDtos,
                    TotalItems = pagedFeedbacks.TotalItems,
                    PageIndex = pagedFeedbacks.PageIndex,
                    PageSize = pagedFeedbacks.PageSize,
                   
                };

                return new ApiResponse<PagingResponse<FeedbackDto>>
                {
                    Data = pagedResponse,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PagingResponse<FeedbackDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<FeedbackDto>> CreateAsync(CreateFeedbackDto createFeedbackDto, Guid currentUserId)
        {
            try
            {
                // Business logic: Validate rating range
                if (createFeedbackDto.Rating < 1 || createFeedbackDto.Rating > 5)
                {
                    return new ApiResponse<FeedbackDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Rating must be between 1 and 5"
                    };
                }

                // Business logic: Check if user has already reviewed this agency
               

                var feedback = new FeedbackEntity
                {
                    Id = Guid.NewGuid(),
                    Content = createFeedbackDto.Content,
                    Rating = createFeedbackDto.Rating,
                    AgencyId = createFeedbackDto.AgencyId,
                    UserId = currentUserId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _feedbackRepository.AddAsync(feedback);
                await _feedbackRepository.SaveChangesAsync();

                var feedbackDto = new FeedbackDto
                {
                    Id = feedback.Id,
                    Content = feedback.Content,
                    Rating = feedback.Rating,
                    CreatedAt = feedback.CreatedAt,
                    UpdatedAt = feedback.UpdatedAt,
                    IsDeleted = feedback.IsDeleted,
                    AgencyId = feedback.AgencyId,
                    UserId = feedback.UserId,
                    UserName = ""
                };

                return new ApiResponse<FeedbackDto>
                {
                    Data = feedbackDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<FeedbackDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<FeedbackDto>> UpdateAsync(Guid id, UpdateFeedbackDto updateFeedbackDto, Guid currentUserId)
        {
            try
            {
                var feedback = await _feedbackRepository.GetByIdAsync(id);
                if (feedback == null)
                {
                    return new ApiResponse<FeedbackDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Feedback not found"
                    };
                }

                // Business logic: Validate rating range
                if (updateFeedbackDto.Rating < 1 || updateFeedbackDto.Rating > 5)
                {
                    return new ApiResponse<FeedbackDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Rating must be between 1 and 5"
                    };
                }

                // Update fields
                if (!string.IsNullOrEmpty(updateFeedbackDto.Content))
                    feedback.Content = updateFeedbackDto.Content;
                
                feedback.Rating = updateFeedbackDto.Rating;

                feedback.UpdatedAt = DateTime.UtcNow;

                await _feedbackRepository.UpdateAsync(feedback);
                await _feedbackRepository.SaveChangesAsync();

                var feedbackDto = new FeedbackDto
                {
                    Id = feedback.Id,
                    Content = feedback.Content,
                    Rating = feedback.Rating,
                    CreatedAt = feedback.CreatedAt,
                    UpdatedAt = feedback.UpdatedAt,
                    IsDeleted = feedback.IsDeleted,
                    AgencyId = feedback.AgencyId,
                    UserId = feedback.UserId,
                    UserName = feedback.User?.Fullname ?? ""
                };

                return new ApiResponse<FeedbackDto>
                {
                    Data = feedbackDto,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<FeedbackDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id, Guid currentUserId)
        {
            try
            {
                var feedback = await _feedbackRepository.GetByIdAsync(id);
                if (feedback == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        MessageId = MessageId.E0000,
                        Message = "Feedback not found"
                    };
                }

                await _feedbackRepository.DeleteAsync(feedback);
                await _feedbackRepository.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = "Feedback deleted successfully"
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

        public async Task<ApiResponse<IEnumerable<FeedbackDto>>> GetByAgencyIdAsync(Guid agencyId)
        {
            try
            {
                var feedbacks = await _feedbackRepository.GetByAgencyIdAsync(agencyId);
                
                var feedbackDtos = feedbacks.Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    Content = f.Content,
                    Rating = f.Rating,
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt,
                    IsDeleted = f.IsDeleted,
                    AgencyId = f.AgencyId,
                    UserId = f.UserId,
                    UserName = f.User?.Fullname ?? ""
                }).ToList();

                return new ApiResponse<IEnumerable<FeedbackDto>>
                {
                    Data = feedbackDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<FeedbackDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<PagingResponse<FeedbackDto>>> GetPagedByAgencyIdAsync(Guid agencyId, int pageNumber, int pageSize)
        {
            try
            {
                var allAgencyFeedbacks = await _feedbackRepository.GetAllAsync();
                var agencyFeedbacks = allAgencyFeedbacks.Where(f => f.AgencyId == agencyId).ToList();
                var totalItems = agencyFeedbacks.Count;
                var items = agencyFeedbacks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                var feedbackDtos = items.Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    Content = f.Content,
                    Rating = f.Rating,
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt,
                    IsDeleted = f.IsDeleted,
                    AgencyId = f.AgencyId,
                    UserId = f.UserId,
                    UserName = f.User?.Fullname ?? ""
                }).ToList();
                var pagedResponse = new PagingResponse<FeedbackDto>
                {
                    Items = feedbackDtos,
                    TotalItems = totalItems,
                    PageIndex = pageNumber,
                    PageSize = pageSize
                };
                return new ApiResponse<PagingResponse<FeedbackDto>>
                {
                    Data = pagedResponse,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PagingResponse<FeedbackDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<FeedbackDto>>> GetByUserIdAsync(Guid userId)
        {
            try
            {
                var feedbacks = await _feedbackRepository.GetByUserIdAsync(userId);
                
                var feedbackDtos = feedbacks.Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    Content = f.Content,
                    Rating = f.Rating,
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt,
                    IsDeleted = f.IsDeleted,
                    AgencyId = f.AgencyId,
                    UserId = f.UserId,
                    UserName = f.User?.Fullname ?? ""
                }).ToList();

                return new ApiResponse<IEnumerable<FeedbackDto>>
                {
                    Data = feedbackDtos,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<FeedbackDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<PagingResponse<FeedbackDto>>> GetPagedByUserIdAsync(Guid userId, int pageNumber, int pageSize)
        {
            try
            {
                var allFeedbacks = await _feedbackRepository.GetAllAsync();
                var userFeedbacks = allFeedbacks.Where(f => f.UserId == userId).ToList();
                var totalItems = userFeedbacks.Count;
                var items = userFeedbacks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                var feedbackDtos = items.Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    Content = f.Content,
                    Rating = f.Rating,
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt,
                    IsDeleted = f.IsDeleted,
                    AgencyId = f.AgencyId,
                    UserId = f.UserId,
                    UserName = f.User?.Fullname ?? ""
                }).ToList();
                var pagedResponse = new PagingResponse<FeedbackDto>
                {
                    Items = feedbackDtos,
                    TotalItems = totalItems,
                    PageIndex = pageNumber,
                    PageSize = pageSize
                };
                return new ApiResponse<PagingResponse<FeedbackDto>>
                {
                    Data = pagedResponse,
                    Success = true,
                    MessageId = MessageId.I0000,
                    Message = Message.GetMessageById(MessageId.I0000)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PagingResponse<FeedbackDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
} 