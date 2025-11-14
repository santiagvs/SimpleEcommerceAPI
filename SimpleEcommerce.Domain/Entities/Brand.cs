using SimpleEcommerce.Domain.Common;
using SimpleEcommerce.Domain.ValueObjects;

namespace SimpleEcommerce.Domain.Entities;

public class Brand : BaseEntity
{
    private Brand() { }
    public string Name { get; private set; } = null!;
    public Slug Slug { get; private set; } = null!;

    public Brand(string name)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Name = name.Trim();
        Slug = Slug.FromName(name);
    }

    public void Rename(string name)
    {
        Name = name.Trim();
        Slug = Slug.FromName(name);
        UpdatedAt = DateTime.UtcNow;
    }
}