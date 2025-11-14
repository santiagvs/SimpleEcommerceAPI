using SimpleEcommerce.Domain.Common;
using SimpleEcommerce.Domain.ValueObjects;

namespace SimpleEcommerce.Domain.Entities;

public class Category : BaseEntity
{
    private Category() { }
    public string Name { get; private set; } = null!;
    public Slug Slug { get; private set; } = null!;
    public Guid? ParentCategoryId { get; private set; }

    public Category(string name, Guid? parentCategoryId = null)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Name = name.Trim();
        Slug = Slug.FromName(name);
        ParentCategoryId = parentCategoryId;
    }

    public void Rename(string name)
    {
        Name = name.Trim();
        Slug = Slug.FromName(name);
        UpdatedAt = DateTime.UtcNow;
    }
}