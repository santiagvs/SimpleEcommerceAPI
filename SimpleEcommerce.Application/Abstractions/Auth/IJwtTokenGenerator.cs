using SimpleEcommerce.Domain.Entities;

namespace SimpleEcommerce.Application.Abstractions.Auth;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}