using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.AgencyServices;
using Snapspot.Application.UseCases.Interfaces.AgencyService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgencyServicesController : ControllerBase
    {
        private readonly IAgencyServiceUseCase _agencyServiceUseCase;

        public AgencyServicesController(IAgencyServiceUseCase agencyServiceUseCase)
        {
            _agencyServiceUseCase = agencyServiceUseCase;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgencyServiceDto>>> GetAll()
        {
            var result = await _agencyServiceUseCase.GetAllAsync();
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AgencyServiceDto>> GetById(Guid id)
        {
            var result = await _agencyServiceUseCase.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("agency/{agencyId}")]
        public async Task<ActionResult<IEnumerable<AgencyServiceDto>>> GetByAgencyId(Guid agencyId)
        {
            var result = await _agencyServiceUseCase.GetByAgencyIdAsync(agencyId);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AgencyServiceDto>> Create([FromBody] CreateAgencyServiceDto createAgencyServiceDto)
        {
            try
            {
                var result = await _agencyServiceUseCase.CreateAsync(createAgencyServiceDto);
                if (!result.Success)
                    return BadRequest(result.Message);

                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
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
                var result = await _agencyServiceUseCase.UpdateAsync(id, updateAgencyServiceDto);
                if (!result.Success)
                    return BadRequest(result.Message);

                return Ok(result.Data);
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
            var result = await _agencyServiceUseCase.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return NoContent();
        }

        [HttpPost("{id}/agencies/{agencyId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddToAgency(Guid id, Guid agencyId)
        {
            try
            {
                var result = await _agencyServiceUseCase.AddToAgencyAsync(id, agencyId);
                if (!result.Success)
                    return BadRequest(result.Message);

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
                var result = await _agencyServiceUseCase.RemoveFromAgencyAsync(id, agencyId);
                if (!result.Success)
                    return BadRequest(result.Message);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 