using Mediator;

namespace SimpleEcommerce.Application.Features.Products.Commands.Create;

public sealed record CreateProductCommand(
    string Sku,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    int Stock,
    Guid BrandId,
    Guid CategoryId
) : ICommand<Guid>;
