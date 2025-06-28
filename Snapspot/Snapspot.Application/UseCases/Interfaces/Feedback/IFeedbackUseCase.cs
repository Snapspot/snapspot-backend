using Snapspot.Application.Models.Agencies;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.UseCases.Interfaces.Feedback
{
    public interface IFeedbackUseCase
    {
        // CRUD Operations
        Task<ApiResponse<FeedbackDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<FeedbackDto>>> GetAllAsync();
        Task<ApiResponse<PagingResponse<FeedbackDto>>> GetPagedAsync(int pageNumber, int pageSize);
        Task<ApiResponse<FeedbackDto>> CreateAsync(CreateFeedbackDto createFeedbackDto, Guid currentUserId);
        Task<ApiResponse<FeedbackDto>> UpdateAsync(Guid id, UpdateFeedbackDto updateFeedbackDto, Guid currentUserId);
        Task<ApiResponse<string>> DeleteAsync(Guid id, Guid currentUserId);
        
        // Business Operations
        Task<ApiResponse<IEnumerable<FeedbackDto>>> GetByAgencyIdAsync(Guid agencyId);
        Task<ApiResponse<PagingResponse<FeedbackDto>>> GetPagedByAgencyIdAsync(Guid agencyId, int pageNumber, int pageSize);
        Task<ApiResponse<IEnumerable<FeedbackDto>>> GetByUserIdAsync(Guid userId);
        Task<ApiResponse<PagingResponse<FeedbackDto>>> GetPagedByUserIdAsync(Guid userId, int pageNumber, int pageSize);
    }
} 