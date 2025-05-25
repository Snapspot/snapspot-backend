using Snapspot.Shared.Common;
using FluentValidation;


namespace Snapspot.WebAPI.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                var response = new ApiResponse<string>
                {
                    Success = false,
                    MessageId = MessageId.E0004,
                    Message = Message.GetMessageById(MessageId.E0004),
                    ListDetailError = ex.Errors.Select(x => new DetailError
                    {
                        Field = x.PropertyName,
                        Value = x.AttemptedValue.ToString() ?? null,
                        Message = x.ErrorMessage,
                        MessageId = MessageId.E0003,
                    }).ToList()
                };

                await httpContext.Response.WriteAsJsonAsync(response);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ValidationExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseValidationExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidationExceptionMiddleware>();
        }
    }
}
