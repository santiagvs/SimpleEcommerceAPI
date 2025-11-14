using SimpleEcommerce.Domain.Common;

namespace SimpleEcommerce.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "BRL")
    {
        if (amount < 0)
            throw new DomainException("Amount cannot be less than 0");
        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency must be ISO 4217 (3 letters)");

        Amount = decimal.Round(amount, 2);
        Currency = currency.ToUpperInvariant();
    }

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Multiply(int qty) => new(Amount * qty, Currency);

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
            throw new DomainException("Currency must be the same");
    }

    public override string ToString() => $"{Currency}{Amount:N2}";
}