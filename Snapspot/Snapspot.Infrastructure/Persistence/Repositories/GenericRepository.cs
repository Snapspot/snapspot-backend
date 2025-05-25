using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Repositories;
using Snapspot.Domain.Base;
using Snapspot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// GenericRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : class, IBaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (asNoTracking)
                query = query.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            if (asNoTracking)
                query = query.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            _ = await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        public Task UpdateAsync(T entity)
        {
            _ = _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            _ = _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteByIdAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _ = _dbSet.Remove(entity);
            }
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task SaveChangesAsync()
        {
            _ = await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithIncludeAsync(Func<IQueryable<T>, IQueryable<T>>? include = null, bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (asNoTracking)
                query = query.AsNoTracking();
            if (include != null)
                query = include(query);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindWithIncludeAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? include = null, bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            if (asNoTracking)
                query = query.AsNoTracking();
            if (include != null)
                query = include(query);
            return await query.ToListAsync();
        }

        public void SaveChanges()
        {
            _ = _context.SaveChanges();
        }

        public async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize, bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (asNoTracking)
                query = query.AsNoTracking();

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> FindPagedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            if (asNoTracking)
                query = query.AsNoTracking();

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> FindWithIncludePagedAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? include,
            int pageNumber,
            int pageSize,
            bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            if (asNoTracking)
                query = query.AsNoTracking();
            if (include != null)
                query = include(query);

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task SoftDeleteAsync(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _ = _context.Set<T>().Attach(entity);
            }

            entity.IsDeleted = true;
            return Task.CompletedTask;
        }

        public async Task SoftDeleteByIdAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await SoftDeleteAsync(entity);
            }
        }

        public Task HardDeleteAsync(T entity)
        {
            _ = _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task HardDeleteByIdAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await HardDeleteAsync(entity);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (asNoTracking)
                query = query.AsNoTracking();
            if (include != null)
                query = include(query);
            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            if (asNoTracking)
                query = query.AsNoTracking();
            if (include != null)
                query = include(query);
            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync();
        }

        public async Task<PagingResponse<T>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (asNoTracking)
                query = query.AsNoTracking();
            if (include != null)
                query = include(query);
            if (orderBy != null)
                query = orderBy(query);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingResponse<T>(items, totalCount, pageNumber, pageSize);
        }

        /// <summary>
        /// Retrieves a paginated list of entities based on the specified filter, with optional includes, ordering, and tracking behavior.
        /// </summary>
        /// <param name="predicate">A filter condition to apply to the query.</param>
        /// <param name="pageNumber">The page number (starting from 1).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="include">Optional function to include related entities (e.g., via .Include()).</param>
        /// <param name="orderBy">Optional function to order the query results.</param>
        /// <param name="asNoTracking">If true, disables tracking for better performance when reading data.</param>
        /// <returns>A <see cref="PagingResponse{T}"/> containing the requested page of items and pagination metadata.</returns>
        public async Task<PagingResponse<T>> FindPagedAsync(
            Expression<Func<T, bool>> predicate,
            int pageNumber,
            int pageSize,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            if (asNoTracking)
                query = query.AsNoTracking();
            if (include != null)
                query = include(query);
            if (orderBy != null)
                query = orderBy(query);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingResponse<T>(items, totalCount, pageNumber, pageSize);
        }
    }
}
