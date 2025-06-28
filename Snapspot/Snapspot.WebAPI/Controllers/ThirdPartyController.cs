using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snapspot.Application.Models.Responses.ThirdParty;
using Snapspot.Application.Models.Spots;
using Snapspot.Application.UseCases.Interfaces.ThirdParty;
using Snapspot.Shared.Common;
using System.Security.Claims;

namespace Snapspot.WebAPI.Controllers
{
    [Route("api/third-party/")]
    [ApiController]
    public class ThirdPartyController : ControllerBase
    {
        private readonly IThirdPartyUseCase _thirdPartyUseCase;

        public ThirdPartyController(IThirdPartyUseCase thirdPartyUseCase) => _thirdPartyUseCase = thirdPartyUseCase;

        [HttpGet("agencies")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SpotDto>>>> GetAgencies()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _thirdPartyUseCase.GetAgencies(userId);

            if (response.Success) return Ok(response);
            return BadRequest(response);

        }

        [HttpGet("sellerpackage-info")]
        public async Task<ActionResult<ApiResponse<GetSerllerPackageInfor>>> GetSellerPackageInfo()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _thirdPartyUseCase.GetSellerPackageInfo(userId);

            if (response.Success) return Ok(response);
            return BadRequest(response);

        }

        [HttpGet("feedbacks")]
        public async Task<ActionResult<ApiResponse<List<GetUserFeedback>>>> GetFeedback()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _thirdPartyUseCase.GetUserFeedback(userId);

            if (response.Success) return Ok(response);
            return BadRequest(response);

        }
    }
}
