using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Application.Features.Auth.Commands;

namespace SimpleEcommerce.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("Register")]
    public async Task<ActionResult> Register(RegisterCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { Message = "User registered successfully" });
    }

    [HttpPost("Login")]
    public async Task<ActionResult<string>> Login(LoginCommand command)
    {
        var token = await _mediator.Send(command);
        return Ok(new { Token = token });
    }
}