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
            stock_quantity,
            slug,
            brand_id,
            category_id,
            status,
            created_at,
            updated_at
        )
        VALUES (@Id, @Sku, @Name, @Description, @Amount, @Stock, @Slug, @BrandId, @CategoryId, @Status, @CreatedAt, @UpdatedAt)
        """;

        using var conn = await _factory.CreateOpenConnectionAsync();
        await conn.ExecuteAsync(sql, new
        {
            product.Id,
            product.Sku,
            product.Name,
            product.Description,
            Stock = product.Stock.Value,
            Slug = product.Slug.Value,
            product.Price.Amount,
            Status = (int)product.Status,
            product.BrandId,
            product.CategoryId,
            product.CreatedAt,
            product.UpdatedAt,
        });
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        const string sql = """
        SELECT
            id,
            sku,
            name,
            description,
            price_amount,
            stock_quantity,
            slug,
            brand_id,
            category_id,
            status,
            created_at,
            updated_at
        FROM products
        WHERE id = @id
        """;

        using var conn = await _factory.CreateOpenConnectionAsync();
        return await conn.QueryFirstOrDefaultAsync<Product>(sql, new { id });
    }

    public async Task<bool> ExistsBySkuAsync(string sku)
    {
        const string sql = "SELECT EXISTS (SELECT 1 FROM products WHERE sku = @sku)";
        using var conn = await _factory.CreateOpenConnectionAsync();
        return await conn.ExecuteScalarAsync<bool>(sql, new { sku });
    }
}
