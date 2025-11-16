using Mediator;
using SimpleEcommerce.Domain.Entities;

namespace SimpleEcommerce.Application.Features.Categories.Queries.GetAll;

public sealed record GetAllCategoriesQuery() : IQuery<IReadOnlyList<Category>>;
