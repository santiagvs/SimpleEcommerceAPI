using SimpleEcommerce.Domain.Entities;

namespace SimpleEcommerce.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<bool> ExistsBySkuAsync(string sku);
}
