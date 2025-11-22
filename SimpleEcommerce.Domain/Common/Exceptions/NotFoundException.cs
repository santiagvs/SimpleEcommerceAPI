namespace SimpleEcommerce.Domain.Common.Exceptions;

public class NotFoundException(string entity, object id)
    : BaseException("NOT_FOUND", $"{entity} with the id ${id} was not found", 404) { }
