using System.Net;
using System.Text.Json;
using SimpleEcommerce.Domain.Common;
using SimpleEcommerce.Domain.Common.Exceptions;

namespace SimpleEcommerce.Presentation.Middlewares;

public class ExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlerMiddleware> logger,
    IWebHostEnvironment env
    )
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger = logger;
    private readonly IWebHostEnvironment _env = env;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleAsync(context, ex);
        }
    }

        private async Task HandleAsync(HttpContext ctx, Exception ex)
    {
        var (status, _, message, errors, logLevel) = MapException(ex);

        using (_logger.BeginScope(new Dictionary<string, object?>
        {
            ["RequestId"] = ctx.TraceIdentifier,
            ["Path"] = ctx.Request.Path
        }))
        {
            if (logLevel == LogLevel.Error) _logger.LogError(ex, "Unhandled exception");
            else _logger.LogWarning(ex, "Handled domain/validation exception");
        }

        var response = new ApiResponse<object>
        {
            Success = false,
            Message = message,
            Errors = errors,
            RequestId = ctx.TraceIdentifier
        };

        ctx.Response.StatusCode = (int)status;
        ctx.Response.ContentType = "application/json";

        if (_env.IsDevelopment() && status == HttpStatusCode.InternalServerError)
        {
            response.Message = $"{message} | {ex.GetType().Name}";
        }

        await ctx.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
    }
    private static (
        HttpStatusCode status,
        string code,
        string message,
        List<ErrorDetail> errors,
        LogLevel logLevel
    ) MapException(Exception ex)
    {
        return ex switch
        {
            FluentValidation.ValidationException fv => (
                HttpStatusCode.BadRequest,
                "VALIDATION_ERROR",
                "Validation failed",
                fv.Errors
                    .Where(e => e is not null)
                    .Select(e => new ErrorDetail
                    {
                        Code = e.ErrorCode,
                        Message = e.ErrorMessage,
                        Property = e.PropertyName
                    })
                    .ToList(),
                LogLevel.Warning
            ),

            DomainException de => (
                HttpStatusCode.BadRequest,
                de.ErrorCode,
                de.Message,
                new List<ErrorDetail> { new() { Code = de.ErrorCode, Message = de.Message } },
                LogLevel.Warning
            ),

            NotFoundException nf => (
                HttpStatusCode.NotFound,
                nf.ErrorCode,
                nf.Message,
                new List<ErrorDetail> { new() { Code = nf.ErrorCode, Message = nf.Message } },
                LogLevel.Warning
            ),

            UnauthorizedException un => (
                HttpStatusCode.Unauthorized,
                un.ErrorCode,
                un.Message,
                new List<ErrorDetail> { new() { Code = un.ErrorCode, Message = un.Message } },
                LogLevel.Warning
            ),

            _ => (
                HttpStatusCode.InternalServerError,
                "INTERNAL_ERROR",
                "An unexpected error has occurred",
                new List<ErrorDetail> { new() { Code = "INTERNAL_ERROR", Message = "An unexpected error has occurred" } },
                LogLevel.Error
            )
        };
    }
}
