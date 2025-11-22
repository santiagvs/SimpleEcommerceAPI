namespace SimpleEcommerce.Domain.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<ErrorDetail>? Errors { get; set; }
    public string? RequestId { get; set; }
}

public class ErrorDetail
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Property { get; set; }
}

public static class ApiResponse
{
    public static ApiResponse<T> Success<T>(T data, string? message = null) => new()
    {
        Success = true,
        Data = data,
        Message = message
    };

    public static ApiResponse<object> Error(string code, string message, string? property = null) => new()
    {
        Success = false,
        Errors = [
            new()
            {
                Code = code,
                Message = message,
                Property = property,
            }
        ]
    };
}
