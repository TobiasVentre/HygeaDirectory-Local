using DirectoryMS.Application.Common;
using DirectoryMS.Common;

namespace DirectoryMS.Middleware;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionMiddleware> _logger;

    public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
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
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (status, code) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "validation_error"),
            NotFoundException => (StatusCodes.Status404NotFound, "not_found"),
            InvalidOperationException => (StatusCodes.Status409Conflict, "domain_conflict"),
            _ => (StatusCodes.Status500InternalServerError, "internal_server_error")
        };

        _logger.LogError(
            exception,
            "Request failed with {Status} and code {Code}. TraceId: {TraceId}",
            status,
            code,
            context.TraceIdentifier);

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";

        var error = new ApiError(
            code,
            exception.Message,
            status,
            context.TraceIdentifier,
            DateTime.UtcNow);

        return context.Response.WriteAsJsonAsync(error);
    }
}
