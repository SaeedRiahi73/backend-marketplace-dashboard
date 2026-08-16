using Azure;
using FluentValidation;
using System.Text.Json;

namespace Task_Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsJsonAsync(new
                {
                    IsSuccess = false,
                    Errors = ex.Errors
                               .Select(x => x.ErrorMessage)
                               .Distinct()
                });
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = new
                {
                    StatusCode = 500,
                    Message = "Internal Server Error"
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
