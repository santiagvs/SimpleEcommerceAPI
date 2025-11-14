using System.Text;
using System.Text.RegularExpressions;
using SimpleEcommerce.Domain.Common;

namespace SimpleEcommerce.Domain.ValueObjects;

public sealed record Slug
{
    private static readonly Regex Valid = new("^[a-z0-9\\-]+$", RegexOptions.Compiled);
    public string Value { get; }
    public Slug(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Valid.IsMatch(value))
            throw new DomainException("Invalid slug");
        Value = value;
    }

    public static Slug FromName(string name)
    {
        var s = name.Trim().ToLowerInvariant();
        var sb = new StringBuilder();
        foreach (var ch in s)
        {
            if (char.IsLetterOrDigit(ch)) sb.Append(ch);
            else if (char.IsWhiteSpace(ch) || ch == '-' || ch == '_') sb.Append('-');
        }
        var cleaned = Regex.Replace(sb.ToString(), "-+", "-").Trim('-');
        if (string.IsNullOrEmpty(cleaned))
            throw new DomainException("Cannot generate slug");

        return new Slug(cleaned);
    }

    public override string ToString() => Value;
}