using Mediator;
using SimpleEcommerce.Domain.Entities;
using SimpleEcommerce.Domain.Interfaces;

namespace SimpleEcommerce.Application.Features.Brands.Queries.GetAll;

public sealed class GetAllBrandsQueryHandler(IBrandRepository repo) : IQueryHandler<GetAllBrandsQuery, IReadOnlyList<Brand>>
{
    private readonly IBrandRepository _repo = repo;
    public async ValueTask<IReadOnlyList<Brand>> Handle(GetAllBrandsQuery query, CancellationToken ct)
        => await _repo.GetAllAsync();
}
