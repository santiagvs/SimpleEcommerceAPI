using Mediator;
using SimpleEcommerce.Domain.Entities;
using SimpleEcommerce.Domain.Interfaces;
using SimpleEcommerce.Domain.ValueObjects;

namespace SimpleEcommerce.Application.Features.Products.Commands.Create;

public sealed class CreateProductCommandHandler(
    IProductRepository productRepository
) : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async ValueTask<Guid> Handle(CreateProductCommand cmd, CancellationToken cancellationToken)
    {
        var sku = new Sku(cmd.Sku);
        var money = new Money(cmd.Price, cmd.Currency);
        var stock = new StockQuantity(cmd.Stock);
        Product product = Product.Create(
            sku,
            cmd.Name,
            cmd.Description,
            money,
            stock,
            cmd.BrandId,
            cmd.CategoryId
        );

        await _productRepository.AddAsync(product);
        return product.Id;
    }
}
