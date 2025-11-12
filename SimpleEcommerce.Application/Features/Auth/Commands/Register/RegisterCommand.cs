
using Mediator;

namespace SimpleEcommerce.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName
) : ICommand<Guid>;