using Microsoft.AspNetCore.Mvc;
using Snapspot.Shared.Common;

namespace Snapspot.WebAPI.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ApiFail(this ControllerBase ctrl, string messageId) =>
            ctrl.BadRequest(new ApiResponse<string>
            {
                Success = false,
                MessageId = messageId,
                Message = Message.GetMessageById(messageId)
            });

        public static IActionResult ApiOk<T>(this ControllerBase ctrl, T data, string messageId = MessageId.I0000) where T : class =>
            ctrl.Ok(new ApiResponse<T>
            {
                Success = true,
                MessageId = messageId,
                Message = Message.GetMessageById(messageId),
                Data = data
            });
    }
}
