using MediatR;
using SimpleEcommerce.Domain.Common;
using SimpleEcommerce.Domain.Interfaces;
using SimpleEcommerce.Domain.Abstractions.Security;
using SimpleEcommerce.Application.Abstractions.Auth;

namespace SimpleEcommerce.Application.Features.Auth.Commands;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator
) : IRequestHandler<LoginCommand, string>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email) ?? throw new DomainException("Invalid credentials");

        var ok = user.Password.Verify(request.Password, _passwordHasher);
        if (!ok) throw new DomainException("Invalid credentials");

        return _jwtTokenGenerator.Generate(user);
    }
}