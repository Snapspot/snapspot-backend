using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Companies;
using Snapspot.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAll()
        {
            var companies = await _companyService.GetAllAsync();
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetById(Guid id)
        {
            var company = await _companyService.GetByIdAsync(id);
            if (company == null)
                return NotFound();

            return Ok(company);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<CompanyDto>> GetByUserId(Guid userId)
        {
            var company = await _companyService.GetByUserIdAsync(userId);
            if (company == null)
                return NotFound();

            return Ok(company);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,ThirdParty")]
        public async Task<ActionResult<CompanyDto>> Create([FromBody] CreateCompanyDto createCompanyDto)
        {
            try
            {
                var company = await _companyService.CreateAsync(createCompanyDto);
                return CreatedAtAction(nameof(GetById), new { id = company.Id }, company);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,ThirdParty")]
        public async Task<ActionResult<CompanyDto>> Update(Guid id, [FromBody] UpdateCompanyDto updateCompanyDto)
        {
            try
            {
                var company = await _companyService.UpdateAsync(id, updateCompanyDto);
                return Ok(company);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Company not found")
                    return NotFound();

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,ThirdParty")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _companyService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
} 