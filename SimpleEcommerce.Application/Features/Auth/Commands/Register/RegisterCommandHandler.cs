using Mediator;
using SimpleEcommerce.Domain.Common;
using SimpleEcommerce.Domain.Entities;
using SimpleEcommerce.Domain.Interfaces;
using SimpleEcommerce.Domain.ValueObjects;
using SimpleEcommerce.Domain.Abstractions.Security;

namespace SimpleEcommerce.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher
) : ICommandHandler<RegisterCommand, Guid>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async ValueTask<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email))
            throw new DomainException("E-mail already exists");

        var email = new Email(request.Email);
        var password = Password.Create(request.Password, _passwordHasher);

        var user = new User(email, password, request.FirstName, request.LastName);

        await _userRepository.AddAsync(user);

        return user.Id;
    }
}