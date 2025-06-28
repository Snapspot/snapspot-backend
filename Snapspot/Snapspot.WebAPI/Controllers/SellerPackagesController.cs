using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.SellerPackages;
using Snapspot.Application.UseCases.Interfaces.SellerPackage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellerPackagesController : ControllerBase
    {
        private readonly ISellerPackageUseCase _sellerPackageUseCase;

        public SellerPackagesController(ISellerPackageUseCase sellerPackageUseCase)
        {
            _sellerPackageUseCase = sellerPackageUseCase;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SellerPackageDto>>> GetAll()
        {
            var sellerPackages = await _sellerPackageUseCase.GetAllAsync();
            return Ok(sellerPackages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SellerPackageDto>> GetById(Guid id)
        {
            var sellerPackage = await _sellerPackageUseCase.GetByIdAsync(id);
            if (sellerPackage == null)
                return NotFound();
            return Ok(sellerPackage);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SellerPackageDto>> Create([FromBody] CreateSellerPackageDto createSellerPackageDto)
        {
            try
            {
                var result = await _sellerPackageUseCase.CreateAsync(createSellerPackageDto);
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
        public async Task<ActionResult<SellerPackageDto>> Update(Guid id, [FromBody] UpdateSellerPackageDto updateSellerPackageDto)
        {
            try
            {
                var result = await _sellerPackageUseCase.UpdateAsync(id, updateSellerPackageDto);
                if (!result.Success || result.Data == null)
                    return BadRequest(result.Message);
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "SellerPackage not found")
                    return NotFound();
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _sellerPackageUseCase.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result.Message);
            return NoContent();
        }
    }
} 