using Microsoft.AspNetCore.Identity;
using ToDoApp.Domain.DTO;

namespace ToDoApp.Domain.Interfaces;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
    Task<TokenDto> CreateToken();
}
