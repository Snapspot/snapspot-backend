using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Repositories;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Persistence.Repositories
{
    public class BookingRepository : GenericRepository<Booking, Guid>, IBookingRepository
    {
        public BookingRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(Guid customerId)
        {
            return await _context.Set<Booking>()
                .Include(b => b.Customer)
                .Include(b => b.Agency)
                .Include(b => b.Service)
                .Where(b => b.CustomerId == customerId && !b.IsDeleted)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByAgencyIdAsync(Guid agencyId)
        {
            return await _context.Set<Booking>()
                .Include(b => b.Customer)
                .Include(b => b.Agency)
                .Include(b => b.Service)
                .Where(b => b.AgencyId == agencyId && !b.IsDeleted)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<bool> IsTimeSlotAvailableAsync(Guid agencyId, DateTime startTime, DateTime endTime)
        {
            var overlappingBookings = await _context.Set<Booking>()
                .Where(b => b.AgencyId == agencyId 
                    && !b.IsDeleted 
                    && b.Status != BookingStatus.Cancelled
                    && ((b.StartTime <= startTime && b.EndTime > startTime)
                    || (b.StartTime < endTime && b.EndTime >= endTime)
                    || (b.StartTime >= startTime && b.EndTime <= endTime)))
                .AnyAsync();

            return !overlappingBookings;
        }
    }
} 