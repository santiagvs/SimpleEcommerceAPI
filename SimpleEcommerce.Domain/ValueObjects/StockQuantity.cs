using SimpleEcommerce.Domain.Common;

namespace SimpleEcommerce.Domain.ValueObjects;

public sealed record StockQuantity
{
    public int Value { get; }
    public StockQuantity(int value)
    {
        if (value < 0) throw new DomainException("Stock cannot be negative");
        Value = value;
    }

    public StockQuantity Add(int qty) => new(Value + qty);
    public StockQuantity Remove(int qty)
    {
        if (qty <= 0) throw new DomainException("Quantity must be positive");
        if (Value - qty < 0) throw new DomainException("Insufficient stock");
        return new StockQuantity(Value - qty);
    }

    public override string ToString() => Value.ToString();
}