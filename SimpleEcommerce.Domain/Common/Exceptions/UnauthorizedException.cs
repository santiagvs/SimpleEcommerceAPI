namespace SimpleEcommerce.Domain.Common.Exceptions;

public class UnauthorizedException(string message = "Unauthorized access")
    : BaseException("UNAUTHORIZED_EXCEPTION", message, 401) { }
