using Dapper;
using SimpleEcommerce.Domain.Entities;
using SimpleEcommerce.Domain.Interfaces;
using SimpleEcommerce.Infrastructure.Persistence;

namespace SimpleEcommerce.Infrastructure.Repository;

public sealed class CategoryRepository(IDbConnectionFactory factory) : ICategoryRepository
{
    private readonly IDbConnectionFactory _factory = factory;

    public async Task AddAsync(Category category)
    {
        const string sql = """
        INSERT INTO categories (id, name, slug, parent_category_id, created_at, updated_at)
        VALUES (@Id, @Name, @Slug, @ParentCategoryId, @CreatedAt, @UpdatedAt);
        """;
        using var conn = await _factory.CreateOpenConnectionAsync();
        await conn.ExecuteAsync(sql, new
        {
            category.Id,
            category.Name,
            Slug = category.Slug.Value,
            category.ParentCategoryId,
            category.CreatedAt,
            category.UpdatedAt
        });
    }

    public async Task<bool> ExistsBySlugAsync(string slug)
    {
        const string sql = "SELECT EXISTS (SELECT 1 FROM categories WHERE slug = @slug);";
        using var conn = await _factory.CreateOpenConnectionAsync();
        return await conn.ExecuteScalarAsync<bool>(sql, new { slug });
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        const string sql = """
        SELECT id, name, slug, parent_category_id, created_at, updated_at
        FROM categories WHERE id = @id;
        """;
        using var conn = await _factory.CreateOpenConnectionAsync();
        var r = await conn.QueryFirstOrDefaultAsync(sql, new { id });
        if (r is null) return null;
        var c = new Category((string)r.name, (Guid?)r.parent_category_id);
        c.GetType().GetProperty(nameof(Category.Id))!.SetValue(c, (Guid)r.id);
        c.GetType().GetProperty(nameof(Category.CreatedAt))!.SetValue(c, (DateTime)r.created_at);
        c.GetType().GetProperty(nameof(Category.UpdatedAt))!.SetValue(c, (DateTime?)r.updated_at);
        return c;
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync()
    {
        const string sql = "SELECT id, name, slug, parent_category_id, created_at, updated_at FROM categories ORDER BY name;";
        using var conn = await _factory.CreateOpenConnectionAsync();
        var rows = await conn.QueryAsync(sql);
        var list = new List<Category>();
        foreach (var r in rows)
        {
            var c = new Category((string)r.name, (Guid?)r.parent_category_id);
            c.GetType().GetProperty(nameof(Category.Id))!.SetValue(c, (Guid)r.id);
            c.GetType().GetProperty(nameof(Category.CreatedAt))!.SetValue(c, (DateTime)r.created_at);
            c.GetType().GetProperty(nameof(Category.UpdatedAt))!.SetValue(c, (DateTime?)r.updated_at);
            list.Add(c);
        }
        return list;
    }
}
