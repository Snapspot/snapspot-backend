using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Models.Responses.ThirdParty;
using Snapspot.Application.Repositories;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly AppDbContext _context;

        public FeedbackRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Feedback> GetByIdAsync(Guid id)
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Agency)
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);
        }

        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Agency)
                .Where(f => !f.IsDeleted)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetByAgencyIdAsync(Guid agencyId)
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Agency)
                .Where(f => f.AgencyId == agencyId && !f.IsDeleted)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Agency)
                .Where(f => f.UserId == userId && !f.IsDeleted)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetApprovedFeedbacksAsync()
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Agency)
                .Where(f => f.IsApproved && !f.IsDeleted)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetPendingFeedbacksAsync()
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Agency)
                .Where(f => !f.IsApproved && !f.IsDeleted)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        // Phân trang
        public async Task<PagingResponse<Feedback>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Agency)
                .Where(f => !f.IsDeleted)
                .OrderByDescending(f => f.CreatedAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingResponse<Feedback>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<PagingResponse<Feedback>> GetByAgencyIdPagedAsync(Guid agencyId, int pageNumber, int pageSize)
        {
            var query = _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Agency)
                .Where(f => f.AgencyId == agencyId && !f.IsDeleted)
                .OrderByDescending(f => f.CreatedAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingResponse<Feedback>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<PagingResponse<Feedback>> GetByUserIdPagedAsync(Guid userId, int pageNumber, int pageSize)
        {
            var query = _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Agency)
                .Where(f => f.UserId == userId && !f.IsDeleted)
                .OrderByDescending(f => f.CreatedAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingResponse<Feedback>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<PagingResponse<Feedback>> GetApprovedFeedbacksPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Agency)
                .Where(f => f.IsApproved && !f.IsDeleted)
                .OrderByDescending(f => f.CreatedAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingResponse<Feedback>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<PagingResponse<Feedback>> GetPendingFeedbacksPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Agency)
                .Where(f => !f.IsApproved && !f.IsDeleted)
                .OrderByDescending(f => f.CreatedAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingResponse<Feedback>(items, totalCount, pageNumber, pageSize);
        }

        public async Task AddAsync(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
        }

        public Task UpdateAsync(Feedback feedback)
        {
            _context.Entry(feedback).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Feedback feedback)
        {
            feedback.IsDeleted = true;
            feedback.UpdatedAt = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Feedbacks.AnyAsync(f => f.Id == id && !f.IsDeleted);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Feedback>> GetFeedbackByCompanyId(Guid companyId)
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Agency)
                .Where(f => f.Agency.CompanyId == companyId)
                .ToListAsync();
        }
    }
} 