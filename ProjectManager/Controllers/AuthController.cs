using BusinessLogic.Mediatr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.Controllers;

[Route("[controller]/[action]")]
[ProducesResponseType(typeof(ErrorDto), 400)]
[Produces("application/json")]
[ResponseCache(NoStore = true, Duration = 0)]
public class AuthController : Controller
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<ActionResult> Register([FromBody] RegisterUser.Command command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);

        return Ok();
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(LoginUser.Response), 200)]
    public async Task<ActionResult<LoginUser.Response>> Login([FromBody] LoginUser.Command command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response);
    }
    
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(LoginUser.Response), 200)]
    public async Task<ActionResult<LoginUser.Response>> RefreshToken(CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();
        var response = await _mediator.Send(new RefreshToken.Command(userId), cancellationToken);

        return Ok(response);
    }
}