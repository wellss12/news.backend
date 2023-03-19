using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSystem.Application.Command;

namespace NewsSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpGet]
    [Route("test")]
    public string Test()
    {
        var jwt_id = User.Claims.FirstOrDefault(p => p.Type == "jti").Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var name = User.FindFirst(ClaimTypes.Name)?.Value;
        var roles = User.FindAll(ClaimTypes.Role);
        return $"{id},{name},{string.Join(",", roles)},  {jwt_id}";
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login(string userName, string password)
    {
        var (result, token) = await _mediator.Send(new LoginCommand(userName, password));
        return result.Succeeded
            ? Ok(token)
            : BadRequest("Login Fail");
    }
    
    [HttpPost]
    [Route("logout")]
    public async Task<ActionResult> Logout(Guid userId)
    {
        await _mediator.Send(new LogoutCommand(userId));
        return Ok();
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> Register(string userName, string password)
    {
        var result = await _mediator.Send(new RegisterCommand(userName, password));
        return result.Succeeded
            ? Ok()
            : BadRequest(result.Errors.Select(error => error.Description));
    }
}