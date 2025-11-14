using Mediator;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Application.Features.Products.Commands.Create;

namespace SimpleEcommerce.Presentation.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ProductController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateProductCommand command)
    {
        Guid productId = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { Id = productId, Message = "Product created successfully" });
    }
}
