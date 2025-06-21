using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Provinces;
using Snapspot.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvincesController : ControllerBase
    {
        private readonly IProvinceService _provinceService;

        public ProvincesController(IProvinceService provinceService)
        {
            _provinceService = provinceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var provinces = await _provinceService.GetAllAsync();
            return Ok(provinces);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProvinceDto>> GetById(Guid id)
        {
            var province = await _provinceService.GetByIdAsync(id);
            if (province == null)
                return NotFound();

            return Ok(province);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProvinceDto>> Create([FromBody] CreateProvinceDto createProvinceDto)
        {
            try
            {
                var province = await _provinceService.CreateAsync(createProvinceDto);
                return CreatedAtAction(nameof(GetById), new { id = province.Id }, province);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProvinceDto>> Update(Guid id, [FromBody] UpdateProvinceDto updateProvinceDto)
        {
            try
            {
                var province = await _provinceService.UpdateAsync(id, updateProvinceDto);
                return Ok(province);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Province not found")
                    return NotFound();

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _provinceService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
} 