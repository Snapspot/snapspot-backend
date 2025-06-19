using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.SellerPackages;
using Snapspot.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellerPackagesController : ControllerBase
    {
        private readonly ISellerPackageService _sellerPackageService;

        public SellerPackagesController(ISellerPackageService sellerPackageService)
        {
            _sellerPackageService = sellerPackageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SellerPackageDto>>> GetAll()
        {
            var sellerPackages = await _sellerPackageService.GetAllAsync();
            return Ok(sellerPackages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SellerPackageDto>> GetById(Guid id)
        {
            var sellerPackage = await _sellerPackageService.GetByIdAsync(id);
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
                var sellerPackage = await _sellerPackageService.CreateAsync(createSellerPackageDto);
                return CreatedAtAction(nameof(GetById), new { id = sellerPackage.Id }, sellerPackage);
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
                var sellerPackage = await _sellerPackageService.UpdateAsync(id, updateSellerPackageDto);
                return Ok(sellerPackage);
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
            var result = await _sellerPackageService.DeleteAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
} 