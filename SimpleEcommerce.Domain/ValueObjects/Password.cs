using SimpleEcommerce.Domain.Abstractions.Security;
using SimpleEcommerce.Domain.Common;

namespace SimpleEcommerce.Domain.ValueObjects;

public sealed record Password
{
    public string Hash { get; }

    private Password(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new DomainException("Invalid password hash");
        Hash = hash;
    }

    public static Password Create(string plainText, IPasswordHasher hasher)
    {
        if (string.IsNullOrWhiteSpace(plainText) || plainText.Length < 6)
            throw new DomainException("Password must be at least 6 characters");

        var hash = hasher.Hash(plainText);
        return new Password(hash);
    }

    public static Password FromHash(string hash) => new(hash);

    public bool Verify(string plainText, IPasswordHasher hasher) => hasher.Verify(Hash, plainText);

    public override string ToString() => "[PROTECTED]";
}