using System.Text.RegularExpressions;
using SimpleEcommerce.Domain.Common.Exceptions;

namespace SimpleEcommerce.Domain.ValueObjects;

public sealed record Sku
{
    private static readonly Regex Pattern = new("^[A-Z0-9\\-]{4,32}$");
    public string Code { get; }
    public Sku(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || !Pattern.IsMatch(code))
            throw new DomainException("Invalid SKU format");

        Code = code.ToUpperInvariant();
    }
    public override string ToString() => Code;
}
