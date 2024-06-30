using BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.WebApi.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError($"error message :{exception.Message} occurs at time {DateTime.UtcNow}");

            (string Title, string Details, int StatusCode) details = exception switch
            {
                InternalServerrException =>
                (
                    exception.GetType().Name,
                    exception.Message,
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
                ValidationException => (
                    exception.GetType().Name,
                    exception.Message,
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                ),

                NotFoundException => (
                    exception.GetType().Name,
                    exception.Message,
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound
                ),

                BadRequestException => (
                   exception.GetType().Name,
                    exception.Message,
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                _ => (
                   exception.GetType().Name,
                    exception.Message,
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
                )

            };

            var problemDetails = new ProblemDetails
            {
                Title = details.Title,
                Detail = details.Details,
                Status = details.StatusCode,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions.Add("TraceId", httpContext.TraceIdentifier);

            if (exception is ValidationException validationexcepion)
                problemDetails.Extensions.Add("ValidationErrors", validationexcepion.ValidationResult.ErrorMessage);

            await httpContext.Response.WriteAsJsonAsync(problemDetails);
            return true;
        }
    }
}
