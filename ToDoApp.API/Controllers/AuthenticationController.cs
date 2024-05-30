using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Commands.LoginUser;
using ToDoApp.Application.Commands.RegisterUser;
using ToDoApp.Domain.DTO;

namespace ToDoApp.API.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly ISender _sender;

    public AuthenticationController(ISender sender) => _sender = sender;

    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto user)
    {
        var result = await _sender.Send(new RegisterUserCommand { UserForRegistration = user });
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors) 
            {
                ModelState.TryAddModelError(error.Code, error.Description); 
            }

            return BadRequest(ModelState);
        }

        return StatusCode(201);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user) 
    {
        var token = await _sender.Send(new LoginUserCommand { UserForAuthentication = user });
        if (string.IsNullOrEmpty(token.AccessToken))
            return Unauthorized();

        return Ok(token.AccessToken);

    }
}
