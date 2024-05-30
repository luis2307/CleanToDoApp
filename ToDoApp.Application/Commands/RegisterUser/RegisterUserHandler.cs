using MediatR;
using Microsoft.AspNetCore.Identity;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.Application.Commands.RegisterUser;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
{
    private readonly IAuthenticationService _authenticationService;
 

    public RegisterUserHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService; 
    }

    public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.RegisterUser(request.UserForRegistration!);
        if (!result.Succeeded)
        {
            return result;
        }

        //await _emailSender.SendEmailAsync(new Message([request.UserForRegistration.Email],
        //   "Registration message", "User successfully registered", null));

        return result;
    }
}
