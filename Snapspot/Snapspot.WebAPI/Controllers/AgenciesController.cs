using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Agencies;
using Snapspot.Application.UseCases.Interfaces.Agency;
using Snapspot.Shared.Common;
using Snapspot.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AgenciesController : ControllerBase
    {
        private readonly IAgencyUseCase _agencyUseCase;

        public AgenciesController(IAgencyUseCase agencyUseCase)
        {
            _agencyUseCase = agencyUseCase;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AgencyDto>>>> GetAll()
        {
            try
            {
                var result = await _agencyUseCase.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<AgencyDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AgencyDto>>> GetById(Guid id)
        {
            try
            {
                var result = await _agencyUseCase.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<AgencyDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AgencyDto>>>> GetByCompanyId(Guid companyId)
        {
            try
            {
                var result = await _agencyUseCase.GetByCompanyIdAsync(companyId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<AgencyDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("spot/{spotId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AgencyDto>>>> GetBySpotId(Guid spotId)
        {
            try
            {
                var result = await _agencyUseCase.GetBySpotIdAsync(spotId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<AgencyDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AgencyDto>>>> Search([FromQuery] string searchTerm)
        {
            try
            {
                var result = await _agencyUseCase.SearchAgenciesAsync(searchTerm);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<AgencyDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.ThirdParty)]
        public async Task<ActionResult<ApiResponse<AgencyDto>>> Create([FromBody] CreateAgencyDto createAgencyDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _agencyUseCase.CreateAsync(createAgencyDto, userId);
            return Ok(result);

        }

        [HttpPut("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult<ApiResponse<AgencyDto>>> Update(Guid id, [FromBody] UpdateAgencyDto updateAgencyDto)
        {
            try
            {
                var result = await _agencyUseCase.UpdateAsync(id, updateAgencyDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<AgencyDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.ThirdParty}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(Guid id)
        {
            try
            {
                var result = await _agencyUseCase.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }
    }
} 
