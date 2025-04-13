using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Common;
using Gateway.DTO.Exceptions;
using System.Net;
using System.Text.Json;

namespace Gateway.Integration.Api.Middleware
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
            var message = exception is ServiceException ? TryParseErrorModel(exception.Message) : SharedConstants.DefaultException;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new ErrorModel
            {
                StatusCode = (int)statusCode,
                message = message,
                Details = exception is ServiceException ? null : exception.ToString()
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
