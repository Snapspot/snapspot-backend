using Snapspot.Application.Models.Agencies;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Services
{
    public interface IFeedbackService
    {
        Task<FeedbackDto> GetByIdAsync(Guid id);
        Task<IEnumerable<FeedbackDto>> GetAllAsync();
        Task<IEnumerable<FeedbackDto>> GetByAgencyIdAsync(Guid agencyId);
        Task<IEnumerable<FeedbackDto>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<FeedbackDto>> GetApprovedFeedbacksAsync();
        Task<IEnumerable<FeedbackDto>> GetPendingFeedbacksAsync();
        
        // Phân trang
        Task<PagingResponse<FeedbackDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<PagingResponse<FeedbackDto>> GetByAgencyIdPagedAsync(Guid agencyId, int pageNumber, int pageSize);
        Task<PagingResponse<FeedbackDto>> GetByUserIdPagedAsync(Guid userId, int pageNumber, int pageSize);
        Task<PagingResponse<FeedbackDto>> GetApprovedFeedbacksPagedAsync(int pageNumber, int pageSize);
        Task<PagingResponse<FeedbackDto>> GetPendingFeedbacksPagedAsync(int pageNumber, int pageSize);
        
        Task<FeedbackDto> CreateAsync(CreateFeedbackDto createFeedbackDto, Guid userId);
        Task<FeedbackDto> UpdateAsync(Guid id, UpdateFeedbackDto updateFeedbackDto, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
        Task<FeedbackDto> ApproveAsync(Guid id, ApproveFeedbackDto approveFeedbackDto);
        Task<bool> ExistsAsync(Guid id);
    }
} 