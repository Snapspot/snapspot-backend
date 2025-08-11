using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Styles;
using Snapspot.Application.UseCases.Interfaces.Style;
using Snapspot.Shared.Common;
using Snapspot.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snapspot.WebAPI.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class StylesController : ControllerBase
    {
        private readonly IStyleUseCase _styleUseCase;

        public StylesController(IStyleUseCase styleUseCase)
        {
            _styleUseCase = styleUseCase;
        }

       
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<StyleDto>>>> GetAll()
        {
            try
            {
                var result = await _styleUseCase.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<StyleDto>>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StyleDto>>> GetById(Guid id)
        {
            try
            {
                var result = await _styleUseCase.GetByIdAsync(id);

                if (result.Success)
                {
                    return Ok(result);
                }

                return NotFound(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<StyleDto>
                {
                    Success = false,
                    MessageId = MessageId.E0000,
                    Message = ex.Message
                });
            }
        }

        
        [HttpGet("category/{category}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<StyleDto>>>> GetByCategory(string category)
        {
            try
            {
                var result = await _styleUseCase.GetByCategoryAsync(category);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<StyleDto>>
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
        public async Task<ActionResult<ApiResponse<StyleDto>>> Create([FromBody] CreateStyleDto createStyleDto)
        {
            try
            {
                if (createStyleDto == null)
                {
                    return BadRequest(new ApiResponse<StyleDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0003,
                        Message = "Dữ liệu không được để trống"
                    });
                }

                var result = await _styleUseCase.CreateAsync(createStyleDto);

                if (result.Success)
                {
                    return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<StyleDto>
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
        public async Task<ActionResult<ApiResponse<StyleDto>>> Update(Guid id, [FromBody] UpdateStyleDto updateStyleDto)
        {
            try
            {
                if (updateStyleDto == null)
                {
                    return BadRequest(new ApiResponse<StyleDto>
                    {
                        Success = false,
                        MessageId = MessageId.E0003,
                        Message = "Dữ liệu không được để trống"
                    });
                }

                var result = await _styleUseCase.UpdateAsync(id, updateStyleDto);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<StyleDto>
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
                var result = await _styleUseCase.DeleteAsync(id);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
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

        /// <summary>
        /// Gán phong cách cho địa điểm
       
        [HttpPost("assign")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult<ApiResponse<string>>> AssignStyleToSpot([FromQuery] Guid styleId, [FromQuery] Guid spotId)
        {
            try
            {
                var result = await _styleUseCase.AssignStyleToSpotAsync(styleId, spotId);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
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

        /// <summary>
        /// Xoa style khỏi địa điểm
        /// 
        [HttpDelete("remove")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult<ApiResponse<string>>> RemoveStyleFromSpot([FromQuery] Guid styleId, [FromQuery] Guid spotId)
        {
            try
            {
                var result = await _styleUseCase.RemoveStyleFromSpotAsync(styleId, spotId);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
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