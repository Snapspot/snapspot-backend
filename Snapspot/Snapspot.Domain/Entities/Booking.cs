using Snapspot.Domain.Base;
using System;
using System.Collections.Generic;

namespace Snapspot.Domain.Entities
{
    public class Booking : BaseEntity<Guid>
    {
        public Guid CustomerId { get; set; }
        public virtual User Customer { get; set; }

        public Guid AgencyId { get; set; }
        public virtual Agency Agency { get; set; }

        public Guid ServiceId { get; set; }
        public virtual AgencyService Service { get; set; }

        public DateTime BookingDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public BookingStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string Notes { get; set; }

        public Booking()
        {
            Status = BookingStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Completed,
        Cancelled
    }
} 