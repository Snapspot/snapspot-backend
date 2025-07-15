using Azure.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Spots;
using Snapspot.Application.UseCases.Interfaces.Spot;
using Snapspot.Shared.Common;
using Snapspot.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpotsController : ControllerBase
    {
        private readonly ISpotUseCase _spotUseCase;

        public SpotsController(ISpotUseCase spotUseCase)
        {
            _spotUseCase = spotUseCase;
        }

        [HttpGet("distance")]
        public async Task<IActionResult> GetAllWithDistance([FromQuery] double latitude, [FromQuery] double longitude)
        {
            try
            {
                var result = await _spotUseCase.GetAllWithDistanceAsync(latitude, longitude);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<SpotDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }


        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<SpotDto>>>> GetAll()
        {
            try
            {
                var result = await _spotUseCase.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<SpotDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SpotDto>>> GetById(Guid id)
        {
            try
            {
                var result = await _spotUseCase.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<SpotDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("district/{districtId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SpotDto>>>> GetByDistrictId(Guid districtId)
        {
            try
            {
                var result = await _spotUseCase.GetByDistrictIdAsync(districtId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<SpotDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("province/{provinceId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SpotDto>>>> GetByProvinceId(Guid provinceId)
        {
            try
            {
                var result = await _spotUseCase.GetByProvinceIdAsync(provinceId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<SpotDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SpotDto>>>> Search([FromQuery] string searchTerm)
        {
            try
            {
                var result = await _spotUseCase.SearchSpotsAsync(searchTerm);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<SpotDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult<ApiResponse<SpotDto>>> Create([FromBody] CreateSpotDto createSpotDto)
        {
            try
            {
                var result = await _spotUseCase.CreateAsync(createSpotDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<SpotDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult<ApiResponse<SpotDto>>> Update(Guid id, [FromBody] UpdateSpotDto updateSpotDto)
        {
            try
            {
                var result = await _spotUseCase.UpdateAsync(id, updateSpotDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<SpotDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult<ApiResponse<string>>> Delete(Guid id)
        {
            try
            {
                var result = await _spotUseCase.DeleteAsync(id);
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