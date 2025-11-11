using MediatR;

namespace SimpleEcommerce.Application.Features.Auth.Commands;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName
) : IRequest<Guid>;