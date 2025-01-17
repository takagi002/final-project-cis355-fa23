using System.Net;
using System.Text.Json;
using UserApi.Exceptions;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (ex.GetType() == typeof(HttpResponseException)) {
                await HandleCustomExceptionAsync(context, (HttpResponseException)ex, _logger);
            } else if (ex.GetType() == typeof(DuplicateEmailException)){
                await HandleOtherExceptionAsync(context, ex, _logger, "Email or username already in use.");
            } else {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ErrorHandlerMiddleware> logger)
    {
        logger.LogError(exception, "An unhandled exception has occurred");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = JsonSerializer.Serialize(new { error = "An unexpected error has occurred" });
        return context.Response.WriteAsync(result);
    }

    private static Task HandleCustomExceptionAsync(HttpContext context, HttpResponseException exception, ILogger<ErrorHandlerMiddleware> logger)
    {
        logger.LogError(exception, exception.Message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception.StatusCode;

        var result = JsonSerializer.Serialize(new { error = exception.Message });
        return context.Response.WriteAsync(result);
    }

    private static Task HandleOtherExceptionAsync(HttpContext context, Exception exception, ILogger<ErrorHandlerMiddleware> logger, string message)
    {
        logger.LogError(exception, message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var result = JsonSerializer.Serialize(new { error = message });
        return context.Response.WriteAsync(result);
    }
}
