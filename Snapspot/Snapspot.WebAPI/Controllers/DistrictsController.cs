using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Districts;
using Snapspot.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistrictsController : ControllerBase
    {
        private readonly IDistrictService _districtService;

        public DistrictsController(IDistrictService districtService)
        {
            _districtService = districtService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DistrictDto>>> GetAll()
        {
            var districts = await _districtService.GetAllAsync();
            return Ok(districts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DistrictDto>> GetById(Guid id)
        {
            var district = await _districtService.GetByIdAsync(id);
            if (district == null)
                return NotFound();

            return Ok(district);
        }

        [HttpGet("province/{provinceId}")]
        public async Task<ActionResult<IEnumerable<DistrictDto>>> GetByProvinceId(Guid provinceId)
        {
            var districts = await _districtService.GetByProvinceIdAsync(provinceId);
            return Ok(districts);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DistrictDto>> Create([FromBody] CreateDistrictDto createDistrictDto)
        {
            try
            {
                var district = await _districtService.CreateAsync(createDistrictDto);
                return CreatedAtAction(nameof(GetById), new { id = district.Id }, district);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DistrictDto>> Update(Guid id, [FromBody] UpdateDistrictDto updateDistrictDto)
        {
            try
            {
                var district = await _districtService.UpdateAsync(id, updateDistrictDto);
                return Ok(district);
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
            var result = await _districtService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
} 