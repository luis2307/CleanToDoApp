using MediatR;
using ToDoApp.Domain.DTO;

namespace ToDoApp.Application.Commands.LoginUser;

public class LoginUserCommand : IRequest<TokenDto>
{
    public UserForAuthenticationDto? UserForAuthentication{ get; set; }
}
