using Mediator;
using SimpleEcommerce.Domain.Common;
using SimpleEcommerce.Domain.Entities;
using SimpleEcommerce.Domain.Interfaces;

namespace SimpleEcommerce.Application.Features.Categories.Commands.Create;

public sealed class CreateCategoryCommandHandler(ICategoryRepository repo) : ICommandHandler<CreateCategoryCommand, Guid>
{
    private readonly ICategoryRepository _repo = repo;

    public async ValueTask<Guid> Handle(CreateCategoryCommand cmd, CancellationToken ct)
    {
        var category = new Category(cmd.Name, cmd.ParentCategoryId);
        if (await _repo.ExistsBySlugAsync(category.Slug.Value))
            throw new DomainException("Category already exists");

        await _repo.AddAsync(category);
        return category.Id;
    }
}
