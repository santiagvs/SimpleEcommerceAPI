namespace SimpleEcommerce.Domain.Common.Exceptions;

public class DomainException(string message) : BaseException("DOMAIN_ERROR", message, 400) { }
