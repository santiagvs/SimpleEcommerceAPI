using Mediator;
using SimpleEcommerce.Domain.Entities;

namespace SimpleEcommerce.Application.Features.Brands.Queries.GetAll;

public sealed record GetAllBrandsQuery() : IQuery<IReadOnlyList<Brand>>;
