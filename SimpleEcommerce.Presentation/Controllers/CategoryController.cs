using Mediator;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Application.Features.Categories.Commands.Create;
using SimpleEcommerce.Application.Features.Categories.Queries.GetAll;
using SimpleEcommerce.Domain.Entities;

namespace SimpleEcommerce.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateCategoryCommand cmd)
    {
        var id = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(Create), new { id });
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Category>>> GetAll()
    {
        var categories = await _mediator.Send(new GetAllCategoriesQuery());
        return Ok(categories);
    }
}
