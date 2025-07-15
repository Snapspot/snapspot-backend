using Snapspot.Application.Repositories;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;
        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

       

        public async Task<Transaction> GetByIdAsync(Guid id)
        {
            return await _context.Transactions
                .Include(t => t.SellerPackage)
                .Include(t => t.Company)
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync(
            Func<IQueryable<Transaction>, IQueryable<Transaction>>? filter = null,
            Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>>? orderBy = null,
            bool disableTracking = false)
        {
            IQueryable<Transaction> query = _context.Transactions
                .Include(t => t.SellerPackage)
                .Include(t => t.Company)
                .Where(t => !t.IsDeleted);

            if (disableTracking)
                query = query.AsNoTracking();

            if (filter != null)
                query = filter(query);

            if (orderBy != null)
                return await orderBy(query).ToListAsync();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByCompanyIdAsync(Guid companyId)
        {
            return await _context.Transactions
                .Include(t => t.SellerPackage)
                .Include(t => t.Company)
                .Where(t => t.CompanyId == companyId && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetBySellerPackageIdAsync(Guid sellerPackageId)
        {
            return await _context.Transactions
                .Include(t => t.SellerPackage)
                .Include(t => t.Company)
                .Where(t => t.SellerPackageId == sellerPackageId && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Transactions
                .Include(t => t.SellerPackage)
                .Include(t => t.Company)
                .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync(Guid companyId)
        {
            return await _context.Transactions
                .Where(t => t.CompanyId == companyId && !t.IsDeleted)
                .SumAsync(t => t.Amount);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transaction>> FindAsync(
            System.Linq.Expressions.Expression<Func<Transaction, bool>> predicate,
            Func<IQueryable<Transaction>, IQueryable<Transaction>>? include = null,
            Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>>? orderBy = null,
            bool asNoTracking = true)
        {
            IQueryable<Transaction> query = _context.Transactions.Where(predicate);
            if (include != null) query = include(query);
            if (asNoTracking) query = query.AsNoTracking();
            if (orderBy != null) return await orderBy(query).ToListAsync();
            return await query.ToListAsync();
        }

        public async Task<Snapspot.Shared.Common.PagingResponse<Transaction>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Func<IQueryable<Transaction>, IQueryable<Transaction>>? include = null,
            Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>>? orderBy = null,
            bool asNoTracking = true)
        {
            throw new NotImplementedException();
        }

        public async Task<Snapspot.Shared.Common.PagingResponse<Transaction>> FindPagedAsync(
            System.Linq.Expressions.Expression<Func<Transaction, bool>> predicate,
            int pageNumber,
            int pageSize,
            Func<IQueryable<Transaction>, IQueryable<Transaction>>? include = null,
            Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>>? orderBy = null,
            bool asNoTracking = true)
        {
            throw new NotImplementedException();
        }

        public async Task<Transaction> AddAsync(Transaction entity)
        {
            await _context.Transactions.AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<Transaction>> AddRangeAsync(IEnumerable<Transaction> entities)
        {
            await _context.Transactions.AddRangeAsync(entities);
            return entities;
        }

        public async Task UpdateAsync(Transaction entity)
        {
            _context.Transactions.Update(entity);
        }

        public async Task UpdateRangeAsync(IEnumerable<Transaction> entities)
        {
            _context.Transactions.UpdateRange(entities);
        }

        public async Task SoftDeleteAsync(Transaction entity)
        {
            entity.IsDeleted = true;
            _context.Transactions.Update(entity);
        }

        public async Task SoftDeleteByIdAsync(Guid id)
        {
            var entity = await _context.Transactions.FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                _context.Transactions.Update(entity);
            }
        }

        public async Task HardDeleteAsync(Transaction entity)
        {
            _context.Transactions.Remove(entity);
        }

        public async Task HardDeleteByIdAsync(Guid id)
        {
            var entity = await _context.Transactions.FindAsync(id);
            if (entity != null)
            {
                _context.Transactions.Remove(entity);
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<Transaction> entities)
        {
            _context.Transactions.RemoveRange(entities);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Transactions.CountAsync();
        }

        public async Task<bool> ExistsAsync(System.Linq.Expressions.Expression<Func<Transaction, bool>> predicate)
        {
            return await _context.Transactions.AnyAsync(predicate);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Transaction>> GetAllWithIncludeAsync(Func<IQueryable<Transaction>, IQueryable<Transaction>>? include = null, bool asNoTracking = true)
        {
            IQueryable<Transaction> query = _context.Transactions;
            if (include != null) query = include(query);
            if (asNoTracking) query = query.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> FindWithIncludeAsync(System.Linq.Expressions.Expression<Func<Transaction, bool>> predicate, Func<IQueryable<Transaction>, IQueryable<Transaction>>? include = null, bool asNoTracking = true)
        {
            IQueryable<Transaction> query = _context.Transactions.Where(predicate);
            if (include != null) query = include(query);
            if (asNoTracking) query = query.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetPagedAsync(int pageNumber, int pageSize, bool asNoTracking = true)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Transaction>> FindPagedAsync(System.Linq.Expressions.Expression<Func<Transaction, bool>> predicate, int pageNumber, int pageSize, bool asNoTracking = true)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Transaction>> FindWithIncludePagedAsync(System.Linq.Expressions.Expression<Func<Transaction, bool>> predicate, Func<IQueryable<Transaction>, IQueryable<Transaction>>? include, int pageNumber, int pageSize, bool asNoTracking = true)
        {
            throw new NotImplementedException();
        }

        public async Task<Transaction?> GetByTransactionCode(string TransactionCode)
        {
            return await _context.Transactions
                .Include(t => t.SellerPackage)
                .Include(t => t.Company)
                .FirstOrDefaultAsync(t => t.TransactionCode == TransactionCode && !t.IsDeleted);
           
        }
    }
} 