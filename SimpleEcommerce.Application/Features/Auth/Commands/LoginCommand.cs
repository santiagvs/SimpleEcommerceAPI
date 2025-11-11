using MediatR;

namespace SimpleEcommerce.Application.Features.Auth.Commands;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<string>;