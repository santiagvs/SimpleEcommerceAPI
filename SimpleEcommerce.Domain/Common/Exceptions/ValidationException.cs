namespace SimpleEcommerce.Domain.Common.Exceptions;

public class ValidationException(Dictionary<string, string[]> errors)
    : BaseException("VALIDATION_ERROR", "One or more validation errors ocurred", 400)
{
    public Dictionary<string, string[]> Errors { get; } = errors;
}
