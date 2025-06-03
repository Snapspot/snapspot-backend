using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Spots;
using Snapspot.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpotsController : ControllerBase
    {
        private readonly ISpotService _spotService;

        public SpotsController(ISpotService spotService)
        {
            _spotService = spotService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpotDto>>> GetAll()
        {
            var spots = await _spotService.GetAllAsync();
            return Ok(spots);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SpotDto>> GetById(Guid id)
        {
            var spot = await _spotService.GetByIdAsync(id);
            if (spot == null)
                return NotFound();

            return Ok(spot);
        }

        [HttpGet("district/{districtId}")]
        public async Task<ActionResult<IEnumerable<SpotDto>>> GetByDistrictId(Guid districtId)
        {
            var spots = await _spotService.GetByDistrictIdAsync(districtId);
            return Ok(spots);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SpotDto>> Create([FromBody] CreateSpotDto createSpotDto)
        {
            try
            {
                var spot = await _spotService.CreateAsync(createSpotDto);
                return CreatedAtAction(nameof(GetById), new { id = spot.Id }, spot);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SpotDto>> Update(Guid id, [FromBody] UpdateSpotDto updateSpotDto)
        {
            try
            {
                var spot = await _spotService.UpdateAsync(id, updateSpotDto);
                return Ok(spot);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Spot not found")
                    return NotFound();

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _spotService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
} 