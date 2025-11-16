using Mediator;
using SimpleEcommerce.Domain.Common;
using SimpleEcommerce.Domain.Entities;
using SimpleEcommerce.Domain.Interfaces;
using SimpleEcommerce.Domain.ValueObjects;

namespace SimpleEcommerce.Application.Features.Brands.Commands.Create;

public sealed class CreateBrandCommandHandler(IBrandRepository repo) : ICommandHandler<CreateBrandCommand, Guid>
{
    private readonly IBrandRepository _repo = repo;

    public async ValueTask<Guid> Handle(CreateBrandCommand cmd, CancellationToken ct)
    {
        var slug = Slug.FromName(cmd.Name);
        if (await _repo.ExistsBySlugAsync(slug.Value))
            throw new DomainException("Brand already exists");

        Brand brand = new(cmd.Name);
        await _repo.AddAsync(brand);
        return brand.Id;
    }
}
