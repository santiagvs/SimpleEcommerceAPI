using Mediator;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Application.Features.Brands.Commands.Create;
using SimpleEcommerce.Application.Features.Brands.Queries.GetAll;
using SimpleEcommerce.Domain.Entities;

namespace SimpleEcommerce.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateBrandCommand cmd)
    {
        var id = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(Create), new { id });
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Brand>>> GetAll()
    {
        var brands = await _mediator.Send(new GetAllBrandsQuery());
        return Ok(brands);
    }
}
