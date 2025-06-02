using Snapspot.Application.Models.Bookings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.Application.Services
{
    public interface IBookingService
    {
        Task<BookingDto> GetByIdAsync(Guid id);
        Task<IEnumerable<BookingDto>> GetAllAsync();
        Task<IEnumerable<BookingDto>> GetBookingsByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<BookingDto>> GetBookingsByAgencyIdAsync(Guid agencyId);
        Task<BookingDto> CreateAsync(CreateBookingDto createBookingDto);
        Task<BookingDto> UpdateAsync(Guid id, UpdateBookingDto updateBookingDto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateStatusAsync(Guid id, string status);
    }
} 