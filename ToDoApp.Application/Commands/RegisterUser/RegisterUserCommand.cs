using MediatR;
using Microsoft.AspNetCore.Identity;
using ToDoApp.Domain.DTO;

namespace ToDoApp.Application.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<IdentityResult>
{
    public UserForRegistrationDto? UserForRegistration { get; set; }
}
