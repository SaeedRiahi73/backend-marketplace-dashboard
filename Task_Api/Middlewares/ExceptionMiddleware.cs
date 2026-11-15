using Azure;
using FluentValidation;
using System.Text.Json;
using Task_Application.Common.Responses;
using Task_Application.Enums;

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

                IEnumerable<string> errors = ex.Errors
                    .Select(error => error.ErrorMessage)
                    .Distinct();

                ResultInfo<object?> response = ResultInfo<object?>.Failure(
                    errors,
                    status: ResultStatus.BadRequest);

                await context.Response.WriteAsJsonAsync(response);
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
