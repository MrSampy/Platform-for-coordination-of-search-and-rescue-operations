using System.Net;
using System.Text.Json;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;

namespace VolunteerService.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var message = exception is VolunteerServiceException ? TryParseErrorModel(exception.Message) : Constants.DefaultException;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new ErrorModel
            {
                StatusCode = (int)statusCode,
                message = message,
                Details = exception is VolunteerServiceException ? null : exception.ToString()
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }

        public static string TryParseErrorModel(string error)
        {
            try
            {
                var errorModel = JsonSerializer.Deserialize<ErrorModel>(error);
                return errorModel?.message ?? error;
            }
            catch (JsonException)
            {
                return error;
            }
        }
    }
}
