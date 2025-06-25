using Snapspot.Domain.Entities;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Repositories
{
    public interface IFeedbackRepository
    {
        Task<Feedback> GetByIdAsync(Guid id);
        Task<IEnumerable<Feedback>> GetAllAsync();
        Task<IEnumerable<Feedback>> GetByAgencyIdAsync(Guid agencyId);
        Task<IEnumerable<Feedback>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Feedback>> GetApprovedFeedbacksAsync();
        Task<IEnumerable<Feedback>> GetPendingFeedbacksAsync();
        
        // Phân trang
        Task<PagingResponse<Feedback>> GetPagedAsync(int pageNumber, int pageSize);
        Task<PagingResponse<Feedback>> GetByAgencyIdPagedAsync(Guid agencyId, int pageNumber, int pageSize);
        Task<PagingResponse<Feedback>> GetByUserIdPagedAsync(Guid userId, int pageNumber, int pageSize);
        Task<PagingResponse<Feedback>> GetApprovedFeedbacksPagedAsync(int pageNumber, int pageSize);
        Task<PagingResponse<Feedback>> GetPendingFeedbacksPagedAsync(int pageNumber, int pageSize);
        
        Task AddAsync(Feedback feedback);
        Task UpdateAsync(Feedback feedback);
        Task DeleteAsync(Feedback feedback);
        Task<bool> ExistsAsync(Guid id);
        Task SaveChangesAsync();
    }
} 