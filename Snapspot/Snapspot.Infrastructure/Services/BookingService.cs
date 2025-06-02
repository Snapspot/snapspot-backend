using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Models.Bookings;
using Snapspot.Application.Repositories;
using Snapspot.Application.Services;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly AppDbContext _context;

        public BookingService(IBookingRepository bookingRepository, AppDbContext context)
        {
            _bookingRepository = bookingRepository;
            _context = context;
        }

        public async Task<BookingDto> GetByIdAsync(Guid id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            return booking != null ? MapToDto(booking) : null;
        }

        public async Task<IEnumerable<BookingDto>> GetAllAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return bookings.Select(MapToDto);
        }

        public async Task<IEnumerable<BookingDto>> GetBookingsByCustomerIdAsync(Guid customerId)
        {
            var bookings = await _bookingRepository.GetBookingsByCustomerIdAsync(customerId);
            return bookings.Select(MapToDto);
        }

        public async Task<IEnumerable<BookingDto>> GetBookingsByAgencyIdAsync(Guid agencyId)
        {
            var bookings = await _bookingRepository.GetBookingsByAgencyIdAsync(agencyId);
            return bookings.Select(MapToDto);
        }

        public async Task<BookingDto> CreateAsync(CreateBookingDto createBookingDto)
        {
            // Validate Customer exists
            var customer = await _context.Users.FirstOrDefaultAsync(u => u.Id == createBookingDto.CustomerId && !u.IsDeleted);
            if (customer == null)
                throw new Exception($"Customer with ID '{createBookingDto.CustomerId}' not found");

            // Validate Agency exists
            var agency = await _context.Agencies.FirstOrDefaultAsync(a => a.Id == createBookingDto.AgencyId && !a.IsDeleted);
            if (agency == null)
                throw new Exception($"Agency with ID '{createBookingDto.AgencyId}' not found");

            // Validate Service exists and belongs to the agency
            var service = await _context.AgencyServices
                .Include(s => s.Agencies)
                .FirstOrDefaultAsync(s => s.Id == createBookingDto.ServiceId && !s.IsDeleted);
            if (service == null)
                throw new Exception($"Service with ID '{createBookingDto.ServiceId}' not found");

            if (!service.Agencies.Any(a => a.Id == createBookingDto.AgencyId))
                throw new Exception($"Service with ID '{createBookingDto.ServiceId}' is not available for Agency with ID '{createBookingDto.AgencyId}'");

            // Check if the time slot is available
            var isTimeSlotAvailable = await _bookingRepository.IsTimeSlotAvailableAsync(
                createBookingDto.AgencyId,
                createBookingDto.StartTime,
                createBookingDto.EndTime);

            if (!isTimeSlotAvailable)
                throw new Exception("The selected time slot is not available");

            var booking = new Booking
            {
                CustomerId = createBookingDto.CustomerId,
                AgencyId = createBookingDto.AgencyId,
                ServiceId = createBookingDto.ServiceId,
                BookingDate = createBookingDto.BookingDate,
                StartTime = createBookingDto.StartTime,
                EndTime = createBookingDto.EndTime,
                TotalPrice = createBookingDto.TotalPrice,
                Notes = createBookingDto.Notes,
                Status = BookingStatus.Pending
            };

            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            return MapToDto(booking);
        }

        public async Task<BookingDto> UpdateAsync(Guid id, UpdateBookingDto updateBookingDto)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
                throw new Exception("Booking not found");

            // If the time slot is being updated, check availability
            if (booking.StartTime != updateBookingDto.StartTime || booking.EndTime != updateBookingDto.EndTime)
            {
                var isTimeSlotAvailable = await _bookingRepository.IsTimeSlotAvailableAsync(
                    booking.AgencyId,
                    updateBookingDto.StartTime,
                    updateBookingDto.EndTime);

                if (!isTimeSlotAvailable)
                    throw new Exception("The selected time slot is not available");
            }

            booking.BookingDate = updateBookingDto.BookingDate;
            booking.StartTime = updateBookingDto.StartTime;
            booking.EndTime = updateBookingDto.EndTime;
            booking.TotalPrice = updateBookingDto.TotalPrice;
            booking.Notes = updateBookingDto.Notes;
            
            if (!string.IsNullOrEmpty(updateBookingDto.Status) && 
                Enum.TryParse<BookingStatus>(updateBookingDto.Status, true, out var status))
            {
                booking.Status = status;
            }

            booking.UpdatedAt = DateTime.UtcNow;

            await _bookingRepository.SaveChangesAsync();

            return MapToDto(booking);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
                return false;

            booking.IsDeleted = true;
            booking.UpdatedAt = DateTime.UtcNow;
            await _bookingRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateStatusAsync(Guid id, string status)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
                return false;

            if (Enum.TryParse<BookingStatus>(status, true, out var bookingStatus))
            {
                booking.Status = bookingStatus;
                booking.UpdatedAt = DateTime.UtcNow;
                await _bookingRepository.SaveChangesAsync();
                return true;
            }

            return false;
        }

        private static BookingDto MapToDto(Booking booking)
        {
            return new BookingDto
            {
                Id = booking.Id,
                CustomerId = booking.CustomerId,
                CustomerName = booking.Customer?.Fullname,
                AgencyId = booking.AgencyId,
                AgencyName = booking.Agency?.Name,
                ServiceId = booking.ServiceId,
                ServiceName = booking.Service?.Name,
                BookingDate = booking.BookingDate,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Status = booking.Status.ToString(),
                TotalPrice = booking.TotalPrice,
                Notes = booking.Notes,
                CreatedAt = booking.CreatedAt,
                UpdatedAt = booking.UpdatedAt
            };
        }
    }
} 