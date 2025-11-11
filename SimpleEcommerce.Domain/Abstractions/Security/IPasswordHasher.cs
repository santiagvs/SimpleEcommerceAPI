namespace SimpleEcommerce.Domain.Abstractions.Security;

public interface IPasswordHasher
{
    string Hash(string plainTextPassword);
    bool Verify(string hash, string plainTextPassword);
}