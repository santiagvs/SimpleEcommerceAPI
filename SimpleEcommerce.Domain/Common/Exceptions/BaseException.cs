namespace SimpleEcommerce.Domain.Common.Exceptions;

public class BaseException : Exception
{
    public string ErrorCode { get; }
    public int StatusCode { get; }

    protected BaseException(string errorCode, string message, int statusCode = 500) : base(message)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }
}
