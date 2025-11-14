using SimpleEcommerce.Domain.Common;
using SimpleEcommerce.Domain.ValueObjects;

namespace SimpleEcommerce.Domain.Entities;

public enum ProductStatus { Draft = 0, Active = 1, Inactive = 2 }

public class Product : BaseEntity
{
    private Product() { }

    public Sku Sku { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public Money Price { get; private set; } = null!;
    public StockQuantity Stock { get; private set; } = null!;
    public Slug Slug { get; private set; } = null!;
    public Guid BrandId { get; private set; }
    public Guid CategoryId { get; private set; }
    public ProductStatus Status { get; private set; }

    public static Product Create(
        Sku sku,
        string name,
        string description,
        Money price,
        StockQuantity stock,
        Guid brandId,
        Guid categoryId
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name required");
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Description required");

        return new Product
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Sku = sku,
            Name = name.Trim(),
            Description = description.Trim(),
            Price = price,
            Stock = stock,
            BrandId = brandId,
            CategoryId = categoryId,
            Slug = Slug.FromName(name),
            Status = ProductStatus.Draft,
        };
    }


    public void Activate()
    {
        Status = ProductStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = ProductStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePrice(Money newPrice)
    {
        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AdjustStock(int delta)
    {
        Stock = delta >= 0 ? Stock.Add(delta) : Stock.Remove(-delta);
        UpdatedAt = DateTime.UtcNow;
    }
}
