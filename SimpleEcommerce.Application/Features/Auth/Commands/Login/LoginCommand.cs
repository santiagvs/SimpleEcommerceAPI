using Mediator;

namespace SimpleEcommerce.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(
    string Email,
    string Password
) : ICommand<string>;