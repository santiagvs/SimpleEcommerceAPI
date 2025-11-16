using Mediator;

namespace SimpleEcommerce.Application.Features.Brands.Commands.Create;

public sealed record CreateBrandCommand(string Name) : ICommand<Guid>;
