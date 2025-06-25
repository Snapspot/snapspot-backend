using Snapspot.Application.Models.Agencies;
using Snapspot.Application.Repositories;
using Snapspot.Application.Services;
using Snapspot.Domain.Entities;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IAgencyRepository _agencyRepository;
        private readonly IUserRepository _userRepository;

        public FeedbackService(
            IFeedbackRepository feedbackRepository,
            IAgencyRepository agencyRepository,
            IUserRepository userRepository)
        {
            _feedbackRepository = feedbackRepository;
            _agencyRepository = agencyRepository;
            _userRepository = userRepository;
        }

        public async Task<FeedbackDto> GetByIdAsync(Guid id)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            return feedback != null ? MapToDto(feedback) : null;
        }

        public async Task<IEnumerable<FeedbackDto>> GetAllAsync()
        {
            var feedbacks = await _feedbackRepository.GetAllAsync();
            return feedbacks.Select(MapToDto);
        }

        public async Task<IEnumerable<FeedbackDto>> GetByAgencyIdAsync(Guid agencyId)
        {
            var feedbacks = await _feedbackRepository.GetByAgencyIdAsync(agencyId);
            return feedbacks.Select(MapToDto);
        }

        public async Task<IEnumerable<FeedbackDto>> GetByUserIdAsync(Guid userId)
        {
            var feedbacks = await _feedbackRepository.GetByUserIdAsync(userId);
            return feedbacks.Select(MapToDto);
        }

        public async Task<IEnumerable<FeedbackDto>> GetApprovedFeedbacksAsync()
        {
            var feedbacks = await _feedbackRepository.GetApprovedFeedbacksAsync();
            return feedbacks.Select(MapToDto);
        }

        public async Task<IEnumerable<FeedbackDto>> GetPendingFeedbacksAsync()
        {
            var feedbacks = await _feedbackRepository.GetPendingFeedbacksAsync();
            return feedbacks.Select(MapToDto);
        }

        // Phân trang
        public async Task<PagingResponse<FeedbackDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var pagedResult = await _feedbackRepository.GetPagedAsync(pageNumber, pageSize);
            return new PagingResponse<FeedbackDto>(
                pagedResult.Items.Select(MapToDto),
                pagedResult.TotalItems,
                pagedResult.PageIndex,
                pagedResult.PageSize
            );
        }

        public async Task<PagingResponse<FeedbackDto>> GetByAgencyIdPagedAsync(Guid agencyId, int pageNumber, int pageSize)
        {
            var pagedResult = await _feedbackRepository.GetByAgencyIdPagedAsync(agencyId, pageNumber, pageSize);
            return new PagingResponse<FeedbackDto>(
                pagedResult.Items.Select(MapToDto),
                pagedResult.TotalItems,
                pagedResult.PageIndex,
                pagedResult.PageSize
            );
        }

        public async Task<PagingResponse<FeedbackDto>> GetByUserIdPagedAsync(Guid userId, int pageNumber, int pageSize)
        {
            var pagedResult = await _feedbackRepository.GetByUserIdPagedAsync(userId, pageNumber, pageSize);
            return new PagingResponse<FeedbackDto>(
                pagedResult.Items.Select(MapToDto),
                pagedResult.TotalItems,
                pagedResult.PageIndex,
                pagedResult.PageSize
            );
        }

        public async Task<PagingResponse<FeedbackDto>> GetApprovedFeedbacksPagedAsync(int pageNumber, int pageSize)
        {
            var pagedResult = await _feedbackRepository.GetApprovedFeedbacksPagedAsync(pageNumber, pageSize);
            return new PagingResponse<FeedbackDto>(
                pagedResult.Items.Select(MapToDto),
                pagedResult.TotalItems,
                pagedResult.PageIndex,
                pagedResult.PageSize
            );
        }

        public async Task<PagingResponse<FeedbackDto>> GetPendingFeedbacksPagedAsync(int pageNumber, int pageSize)
        {
            var pagedResult = await _feedbackRepository.GetPendingFeedbacksPagedAsync(pageNumber, pageSize);
            return new PagingResponse<FeedbackDto>(
                pagedResult.Items.Select(MapToDto),
                pagedResult.TotalItems,
                pagedResult.PageIndex,
                pagedResult.PageSize
            );
        }

        public async Task<FeedbackDto> CreateAsync(CreateFeedbackDto createFeedbackDto, Guid userId)
        {
            // Kiểm tra agency có tồn tại không
            var agency = await _agencyRepository.GetByIdAsync(createFeedbackDto.AgencyId);
            if (agency == null)
                throw new ArgumentException("Agency không tồn tại");

            // Kiểm tra user có tồn tại không
            var user = await _userRepository.GetByUserIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User không tồn tại");

            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                Content = createFeedbackDto.Content,
                Rating = createFeedbackDto.Rating,
                IsApproved = false, // Mặc định chưa được phê duyệt
                UserId = userId,
                AgencyId = createFeedbackDto.AgencyId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _feedbackRepository.AddAsync(feedback);
            await _feedbackRepository.SaveChangesAsync();

            // Cập nhật rating trung bình cho agency
            await UpdateAgencyRating(createFeedbackDto.AgencyId);

            return MapToDto(feedback);
        }

        public async Task<FeedbackDto> UpdateAsync(Guid id, UpdateFeedbackDto updateFeedbackDto, Guid userId)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null)
                throw new ArgumentException("Feedback không tồn tại");

            // Kiểm tra quyền: chỉ user tạo feedback mới được cập nhật
            if (feedback.UserId != userId)
                throw new UnauthorizedAccessException("Bạn không có quyền cập nhật feedback này");

            // Chỉ cho phép cập nhật feedback chưa được phê duyệt
            if (feedback.IsApproved)
                throw new InvalidOperationException("Không thể cập nhật feedback đã được phê duyệt");

            feedback.Content = updateFeedbackDto.Content;
            feedback.Rating = updateFeedbackDto.Rating;
            feedback.UpdatedAt = DateTime.UtcNow;

            await _feedbackRepository.UpdateAsync(feedback);
            await _feedbackRepository.SaveChangesAsync();

            // Cập nhật rating trung bình cho agency
            await UpdateAgencyRating(feedback.AgencyId);

            return MapToDto(feedback);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null)
                return false;

            // Kiểm tra quyền: chỉ user tạo feedback mới được xóa
            if (feedback.UserId != userId)
                throw new UnauthorizedAccessException("Bạn không có quyền xóa feedback này");

            await _feedbackRepository.DeleteAsync(feedback);
            await _feedbackRepository.SaveChangesAsync();

            // Cập nhật rating trung bình cho agency
            await UpdateAgencyRating(feedback.AgencyId);

            return true;
        }

        public async Task<FeedbackDto> ApproveAsync(Guid id, ApproveFeedbackDto approveFeedbackDto)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null)
                throw new ArgumentException("Feedback không tồn tại");

            feedback.IsApproved = approveFeedbackDto.IsApproved;
            feedback.UpdatedAt = DateTime.UtcNow;

            await _feedbackRepository.UpdateAsync(feedback);
            await _feedbackRepository.SaveChangesAsync();

            // Cập nhật rating trung bình cho agency
            await UpdateAgencyRating(feedback.AgencyId);

            return MapToDto(feedback);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _feedbackRepository.ExistsAsync(id);
        }

        private static FeedbackDto MapToDto(Feedback feedback)
        {
            return new FeedbackDto
            {
                Id = feedback.Id,
                Content = feedback.Content,
                Rating = feedback.Rating,
                IsApproved = feedback.IsApproved,
                UserId = feedback.UserId,
                UserName = feedback.User?.Fullname,
                AgencyId = feedback.AgencyId,
                CreatedAt = feedback.CreatedAt,
                UpdatedAt = feedback.UpdatedAt,
                IsDeleted = feedback.IsDeleted
            };
        }

        private async Task UpdateAgencyRating(Guid agencyId)
        {
            var agency = await _agencyRepository.GetByIdAsync(agencyId);
            if (agency != null)
            {
                var approvedFeedbacks = await _feedbackRepository.GetByAgencyIdAsync(agencyId);
                var approvedFeedbacksList = approvedFeedbacks.Where(f => f.IsApproved).ToList();

                if (approvedFeedbacksList.Any())
                {
                    var averageRating = approvedFeedbacksList.Average(f => f.Rating);
                    agency.Rating = (float)averageRating;
                    agency.UpdatedAt = DateTime.UtcNow;

                    await _agencyRepository.UpdateAsync(agency);
                    await _agencyRepository.SaveChangesAsync();
                }
            }
        }
    }
} 