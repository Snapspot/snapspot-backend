using System;

namespace Snapspot.Application.Models.Bookings
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid AgencyId { get; set; }
        public string AgencyName { get; set; }
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateBookingDto
    {
        public Guid CustomerId { get; set; }
        public Guid AgencyId { get; set; }
        public Guid ServiceId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateBookingDto
    {
        public DateTime BookingDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string Notes { get; set; }
    }
} 