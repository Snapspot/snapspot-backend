using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Provinces;
using Snapspot.Application.UseCases.Interfaces.Province;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvincesController : ControllerBase
    {
        private readonly IProvinceUseCase _provinceUseCase;

        public ProvincesController(IProvinceUseCase provinceUseCase)
        {
            _provinceUseCase = provinceUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var provinces = await _provinceUseCase.GetAllAsync();
            return Ok(provinces);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProvinceDto>> GetById(Guid id)
        {
            var province = await _provinceUseCase.GetByIdAsync(id);
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
                var result = await _provinceUseCase.CreateAsync(createProvinceDto);
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
        public async Task<ActionResult<ProvinceDto>> Update(Guid id, [FromBody] UpdateProvinceDto updateProvinceDto)
        {
            try
            {
                var result = await _provinceUseCase.UpdateAsync(id, updateProvinceDto);
                if (!result.Success || result.Data == null)
                    return BadRequest(result.Message);
                return Ok(result.Data);
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
            var result = await _provinceUseCase.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result.Message);
            return NoContent();
        }
    }
} 