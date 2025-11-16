using SimpleEcommerce.Domain.Entities;

namespace SimpleEcommerce.Domain.Interfaces;

public interface IBrandRepository : IRepository<Brand>, IListRepository<Brand>
{
    Task<bool> ExistsBySlugAsync(string slug);
}
