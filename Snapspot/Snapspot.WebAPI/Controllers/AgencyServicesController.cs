using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.AgencyServices;
using Snapspot.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgencyServicesController : ControllerBase
    {
        private readonly IAgencyServiceService _agencyServiceService;

        public AgencyServicesController(IAgencyServiceService agencyServiceService)
        {
            _agencyServiceService = agencyServiceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgencyServiceDto>>> GetAll()
        {
            var services = await _agencyServiceService.GetAllAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AgencyServiceDto>> GetById(Guid id)
        {
            var service = await _agencyServiceService.GetByIdAsync(id);
            if (service == null)
                return NotFound();

            return Ok(service);
        }

        [HttpGet("agency/{agencyId}")]
        public async Task<ActionResult<IEnumerable<AgencyServiceDto>>> GetByAgencyId(Guid agencyId)
        {
            var services = await _agencyServiceService.GetByAgencyIdAsync(agencyId);
            return Ok(services);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AgencyServiceDto>> Create([FromBody] CreateAgencyServiceDto createAgencyServiceDto)
        {
            try
            {
                var service = await _agencyServiceService.CreateAsync(createAgencyServiceDto);
                return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AgencyServiceDto>> Update(Guid id, [FromBody] UpdateAgencyServiceDto updateAgencyServiceDto)
        {
            try
            {
                var service = await _agencyServiceService.UpdateAsync(id, updateAgencyServiceDto);
                return Ok(service);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _agencyServiceService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/agencies/{agencyId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddToAgency(Guid id, Guid agencyId)
        {
            try
            {
                await _agencyServiceService.AddToAgencyAsync(id, agencyId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/agencies/{agencyId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveFromAgency(Guid id, Guid agencyId)
        {
            try
            {
                await _agencyServiceService.RemoveFromAgencyAsync(id, agencyId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 