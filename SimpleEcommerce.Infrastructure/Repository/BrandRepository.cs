using Dapper;
using SimpleEcommerce.Domain.Entities;
using SimpleEcommerce.Domain.Interfaces;
using SimpleEcommerce.Infrastructure.Persistence;

namespace SimpleEcommerce.Infrastructure.Repository;

public sealed class BrandRepository(IDbConnectionFactory factory) : IBrandRepository
{
    private readonly IDbConnectionFactory _factory = factory;

    public async Task AddAsync(Brand brand)
    {
        const string sql = """
        INSERT INTO brands (id, name, slug, created_at, updated_at)
        VALUES (@Id, @Name, @Slug, @CreatedAt, @UpdatedAt);
        """;
        using var conn = await _factory.CreateOpenConnectionAsync();
        await conn.ExecuteAsync(sql, new
        {
            brand.Id,
            brand.Name,
            Slug = brand.Slug.Value,
            brand.CreatedAt,
            brand.UpdatedAt
        });
    }

    public async Task<bool> ExistsBySlugAsync(string slug)
    {
        const string sql = "SELECT EXISTS (SELECT 1 FROM brands WHERE slug = @slug);";
        using var conn = await _factory.CreateOpenConnectionAsync();
        return await conn.ExecuteScalarAsync<bool>(sql, new { slug });
    }

    public async Task<Brand?> GetByIdAsync(Guid id)
    {
        const string sql = """
        SELECT id, name, slug, created_at, updated_at
        FROM brands WHERE id = @id;
        """;
        using var conn = await _factory.CreateOpenConnectionAsync();
        var row = await conn.QueryFirstOrDefaultAsync(sql, new { id });
        if (row is null) return null;
        var brand = new Brand((string)row.name);
        brand.GetType().GetProperty(nameof(Brand.Id))!.SetValue(brand, (Guid)row.id);
        brand.GetType().GetProperty(nameof(Brand.CreatedAt))!.SetValue(brand, (DateTime)row.created_at);
        brand.GetType().GetProperty(nameof(Brand.UpdatedAt))!.SetValue(brand, (DateTime?)row.updated_at);
        return brand;
    }

    public async Task<IReadOnlyList<Brand>> GetAllAsync()
    {
        const string sql = "SELECT id, name, slug, created_at, updated_at FROM brands ORDER BY name;";
        using var conn = await _factory.CreateOpenConnectionAsync();
        var rows = await conn.QueryAsync(sql);
        var list = new List<Brand>();
        foreach (var r in rows)
        {
            var b = new Brand((string)r.name);
            b.GetType().GetProperty(nameof(Brand.Id))!.SetValue(b, (Guid)r.id);
            b.GetType().GetProperty(nameof(Brand.CreatedAt))!.SetValue(b, (DateTime)r.created_at);
            b.GetType().GetProperty(nameof(Brand.UpdatedAt))!.SetValue(b, (DateTime?)r.updated_at);
            list.Add(b);
        }
        return list;
    }
}
