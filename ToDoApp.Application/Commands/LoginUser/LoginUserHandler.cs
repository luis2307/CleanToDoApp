using MediatR;
using ToDoApp.Domain.DTO;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.Application.Commands.LoginUser;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, TokenDto>
{
    private readonly IAuthenticationService _authenticationService;

    public LoginUserHandler(IAuthenticationService authenticationService) => 
        _authenticationService = authenticationService;

    public async Task<TokenDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        if (!await _authenticationService.ValidateUser(request.UserForAuthentication!))
            return new TokenDto(string.Empty);

        return await _authenticationService.CreateToken();
    }
}
