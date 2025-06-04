using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Agencies;
using Snapspot.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgenciesController : ControllerBase
    {
        private readonly IAgencyManagementService _agencyService;
        private readonly IAgencyServiceService _agencyServiceService;

        public AgenciesController(IAgencyManagementService agencyService, IAgencyServiceService agencyServiceService)
        {
            _agencyService = agencyService;
            _agencyServiceService = agencyServiceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgencyDto>>> GetAll()
        {
            var agencies = await _agencyService.GetAllAsync();
            return Ok(agencies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AgencyDto>> GetById(Guid id)
        {
            var agency = await _agencyService.GetByIdAsync(id);
            if (agency == null)
                return NotFound();

            return Ok(agency);
        }

        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<IEnumerable<AgencyDto>>> GetByCompanyId(Guid companyId)
        {
            var agencies = await _agencyService.GetByCompanyIdAsync(companyId);
            return Ok(agencies);
        }

        [HttpGet("spot/{spotId}")]
        public async Task<ActionResult<IEnumerable<AgencyDto>>> GetBySpotId(Guid spotId)
        {
            var agencies = await _agencyService.GetBySpotIdAsync(spotId);
            return Ok(agencies);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,ThirdParty")]
        public async Task<ActionResult<AgencyDto>> Create([FromBody] CreateAgencyDto createAgencyDto)
        {
            try
            {
                var agency = await _agencyService.CreateAsync(createAgencyDto);
                return CreatedAtAction(nameof(GetById), new { id = agency.Id }, agency);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,ThirdParty")]
        public async Task<ActionResult<AgencyDto>> Update(Guid id, [FromBody] UpdateAgencyDto updateAgencyDto)
        {
            try
            {
                var agency = await _agencyService.UpdateAsync(id, updateAgencyDto);
                return Ok(agency);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Agency not found")
                    return NotFound();

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,ThirdParty")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _agencyService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/services/{serviceId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,ThirdParty")]
        public async Task<ActionResult> AddService(Guid id, Guid serviceId)
        {
            try
            {
                await _agencyServiceService.AddToAgencyAsync(serviceId, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/services/{serviceId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,ThirdParty")]
        public async Task<ActionResult> RemoveService(Guid id, Guid serviceId)
        {
            try
            {
                await _agencyServiceService.RemoveFromAgencyAsync(serviceId, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/services")]
        public async Task<ActionResult> GetServices(Guid id)
        {
            try
            {
                var services = await _agencyServiceService.GetByAgencyIdAsync(id);
                return Ok(services);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 