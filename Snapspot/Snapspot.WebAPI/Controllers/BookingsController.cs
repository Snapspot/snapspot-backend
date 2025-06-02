using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Bookings;
using Snapspot.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAll()
        {
            var bookings = await _bookingService.GetAllAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetById(Guid id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetByCustomerId(Guid customerId)
        {
            var bookings = await _bookingService.GetBookingsByCustomerIdAsync(customerId);
            return Ok(bookings);
        }

        [HttpGet("agency/{agencyId}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetByAgencyId(Guid agencyId)
        {
            var bookings = await _bookingService.GetBookingsByAgencyIdAsync(agencyId);
            return Ok(bookings);
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> Create(CreateBookingDto createBookingDto)
        {
            try
            {
                var booking = await _bookingService.CreateAsync(createBookingDto);
                return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookingDto>> Update(Guid id, UpdateBookingDto updateBookingDto)
        {
            try
            {
                var booking = await _bookingService.UpdateAsync(id, updateBookingDto);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateStatus(Guid id, [FromBody] string status)
        {
            var result = await _bookingService.UpdateStatusAsync(id, status);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _bookingService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
} 