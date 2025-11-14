using SimpleEcommerce.Domain.Entities;

namespace SimpleEcommerce.Domain.Interfaces;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task<Product?> GetByIdAsync(Guid id);
    Task<bool> ExistsBySkuAsync(string sku);
}
