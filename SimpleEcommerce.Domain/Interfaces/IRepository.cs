using SimpleEcommerce.Domain.Common;

namespace SimpleEcommerce.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entity);
}

public interface IListRepository<TEntity>
{
    Task<IReadOnlyList<TEntity>> GetAllAsync();
}
