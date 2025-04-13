using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace OperationsService.API.Middleware
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
            var message = exception is OperationsServiceException ? TryParseErrorModel(exception.Message) : Constants.DefaultException;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new ErrorModel
            {
                StatusCode = (int)statusCode,
                message = message,
                Details = exception is OperationsServiceException ? null : exception.ToString()
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
