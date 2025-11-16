using SimpleEcommerce.Domain.Entities;

namespace SimpleEcommerce.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>, IListRepository<Category>
{
    Task<bool> ExistsBySlugAsync(string slug);
}
