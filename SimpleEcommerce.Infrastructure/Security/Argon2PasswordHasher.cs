using Isopoh.Cryptography.Argon2;
using SimpleEcommerce.Domain.Abstractions.Security;

namespace SimpleEcommerce.Infrastructure.Security;

public sealed class Argon2PasswordHasher : IPasswordHasher
{
    public string Hash(string plainTextPassword) => Argon2.Hash(plainTextPassword);

    public bool Verify(string hash, string plainTextPassword) => Argon2.Verify(hash, plainTextPassword);
}