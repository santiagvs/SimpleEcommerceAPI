using Mediator;

namespace SimpleEcommerce.Application.Features.Categories.Commands.Create;

public sealed record CreateCategoryCommand(string Name, Guid? ParentCategoryId) : ICommand<Guid>;
