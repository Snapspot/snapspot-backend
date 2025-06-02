using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Repositories
{
    public interface IBookingRepository : IGenericRepository<Booking, Guid>
    {
        Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<Booking>> GetBookingsByAgencyIdAsync(Guid agencyId);
        Task<bool> IsTimeSlotAvailableAsync(Guid agencyId, DateTime startTime, DateTime endTime);
    }
} 