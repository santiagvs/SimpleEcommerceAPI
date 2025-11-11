using SimpleEcommerce.Domain.Common;

namespace SimpleEcommerce.Domain.ValueObjects;

public sealed record Email
{
    public string Address { get; }
    public Email(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("E-mail cannot be empty");

        if (!IsValidEmail(address))
            throw new DomainException("Invalid e-mail format");

        Address = address.ToLower();
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static implicit operator string(Email email) => email.Address;
    public static explicit operator Email(string address) => new(address);

    public override string ToString() => Address;
}