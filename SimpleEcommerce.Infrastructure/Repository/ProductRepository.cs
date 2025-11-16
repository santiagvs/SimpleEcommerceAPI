using Dapper;
using SimpleEcommerce.Domain.Entities;
using SimpleEcommerce.Domain.Interfaces;
using SimpleEcommerce.Infrastructure.Persistence;

namespace SimpleEcommerce.Infrastructure.Repository;

public sealed class ProductRepository(IDbConnectionFactory factory) : IProductRepository
{
    private readonly IDbConnectionFactory _factory = factory;

    public async Task AddAsync(Product product)
    {
        const string sql = """
        INSERT INTO products (
            id,
            sku,
            name,
            description,
            price_amount,
            price_currency,
            stock,
            slug,
            brand_id,
            category_id,
            status,
            created_at,
            updated_at
        )
        VALUES (
            @Id,
            @Sku,
            @Name,
            @Description,
            @PriceAmount,
            @PriceCurrency,
            @Stock,
            @Slug,
            @BrandId,
            @CategoryId,
            @Status,
            @CreatedAt,
            @UpdatedAt
        );
        """;

        using var conn = await _factory.CreateOpenConnectionAsync();
        await conn.ExecuteAsync(sql, new
        {
            product.Id,
            Sku = product.Sku.Code,
            product.Name,
            product.Description,
            PriceAmount = product.Price.Amount,
            PriceCurrency = product.Price.Currency,
            Stock = product.Stock.Value,
            Slug = product.Slug.Value,
            product.BrandId,
            product.CategoryId,
            Status = (int)product.Status,
            product.CreatedAt,
            product.UpdatedAt,
        });
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        const string sql = """
        SELECT
            id, sku, name, description,
            price_amount, price_currency,
            stock, slug, brand_id, category_id,
            status, created_at, updated_at
        FROM products
        WHERE id = @id;
        """;

        using var conn = await _factory.CreateOpenConnectionAsync();
        var r = await conn.QueryFirstOrDefaultAsync(sql, new { id });
        if (r is null) return null;

        var p = Product.Create(
            new Domain.ValueObjects.Sku((string)r.sku),
            (string)r.name,
            (string)r.description,
            new Domain.ValueObjects.Money((decimal)r.price_amount, (string)r.price_currency),
            new Domain.ValueObjects.StockQuantity((int)r.stock),
            (Guid)r.brand_id,
            (Guid)r.category_id
        );
        p.GetType().GetProperty(nameof(Product.Id))!.SetValue(p, (Guid)r.id);
        p.GetType().GetProperty(nameof(Product.CreatedAt))!.SetValue(p, (DateTime)r.created_at);
        p.GetType().GetProperty(nameof(Product.UpdatedAt))!.SetValue(p, (DateTime?)r.updated_at);
        return p;
    }

    public async Task<bool> ExistsBySkuAsync(string sku)
    {
        const string sql = "SELECT EXISTS (SELECT 1 FROM products WHERE sku = @sku);";
        using var conn = await _factory.CreateOpenConnectionAsync();
        return await conn.ExecuteScalarAsync<bool>(sql, new { sku });
    }
}
