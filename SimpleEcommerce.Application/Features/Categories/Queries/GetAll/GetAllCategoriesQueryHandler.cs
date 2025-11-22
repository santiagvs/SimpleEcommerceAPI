using Mediator;
using SimpleEcommerce.Domain.Entities;
using SimpleEcommerce.Domain.Interfaces;

namespace SimpleEcommerce.Application.Features.Categories.Queries.GetAll;

public sealed class GetAllCategoriesQueryHandler(ICategoryRepository repo) : IQueryHandler<GetAllCategoriesQuery, IReadOnlyList<Category>>
{
    private readonly ICategoryRepository _repo = repo;

    public async ValueTask<IReadOnlyList<Category>> Handle(GetAllCategoriesQuery query, CancellationToken ct)
        => await _repo.GetAllAsync();
}
