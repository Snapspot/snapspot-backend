using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Companies;
using Snapspot.Application.UseCases.Interfaces.Company;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyUseCase _companyUseCase;

        public CompaniesController(ICompanyUseCase companyUseCase)
        {
            _companyUseCase = companyUseCase;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAll()
        {
            var companies = await _companyUseCase.GetAllAsync();
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetById(Guid id)
        {
            var company = await _companyUseCase.GetByIdAsync(id);
            if (company == null)
                return NotFound();

            return Ok(company);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<CompanyDto>> GetByUserId(Guid userId)
        {
            // Đã loại bỏ khỏi usecase, nếu cần thì implement lại ở usecase, tạm thời comment
            // var company = await _companyUseCase.GetByUserIdAsync(userId);
            // if (company == null)
            //     return NotFound();
            // return Ok(company);
            return NotFound();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,ThirdParty")]
        public async Task<ActionResult<CompanyDto>> Create([FromBody] CreateCompanyDto createCompanyDto)
        {
            try
            {
                var result = await _companyUseCase.CreateAsync(createCompanyDto);
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
        [Authorize(Roles = "Admin,ThirdParty")]
        public async Task<ActionResult<CompanyDto>> Update(Guid id, [FromBody] UpdateCompanyDto updateCompanyDto)
        {
            try
            {
                var result = await _companyUseCase.UpdateAsync(id, updateCompanyDto);
                if (!result.Success || result.Data == null)
                    return BadRequest(result.Message);
                return Ok(result.Data);
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
            var result = await _companyUseCase.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result.Message);
            return NoContent();
        }
    }
} 