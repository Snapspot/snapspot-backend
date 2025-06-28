using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Districts;
using Snapspot.Application.UseCases.Interfaces.District;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistrictsController : ControllerBase
    {
        private readonly IDistrictUseCase _districtUseCase;

        public DistrictsController(IDistrictUseCase districtUseCase)
        {
            _districtUseCase = districtUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var districts = await _districtUseCase.GetAllAsync();
            return Ok(districts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DistrictDto>> GetById(Guid id)
        {
            var district = await _districtUseCase.GetByIdAsync(id);
            if (district == null)
                return NotFound();

            return Ok(district);
        }

        [HttpGet("province/{provinceId}")]
        public async Task<ActionResult<IEnumerable<DistrictDto>>> GetByProvinceId(Guid provinceId)
        {
            var districts = await _districtUseCase.GetByProvinceIdAsync(provinceId);
            return Ok(districts);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DistrictDto>> Create([FromBody] CreateDistrictDto createDistrictDto)
        {
            try
            {
                var result = await _districtUseCase.CreateAsync(createDistrictDto);
                if (!result.Success || result.Data == null)
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
        public async Task<ActionResult<DistrictDto>> Update(Guid id, [FromBody] UpdateDistrictDto updateDistrictDto)
        {
            try
            {
                var result = await _districtUseCase.UpdateAsync(id, updateDistrictDto);
                if (!result.Success || result.Data == null)
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
            var result = await _districtUseCase.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result.Message);
            return NoContent();
        }
    }
} 