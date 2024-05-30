using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoApp.Domain.DTO;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.Infrastructure.AuthService;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    private User? _user;

    public AuthenticationService(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
    {
        var user = new User
        {
            Email = userForRegistration.Email,
            FirstName = userForRegistration.FirstName,
            LastName = userForRegistration.LastName,
            PhoneNumber = userForRegistration.PhoneNumber,
            UserName = userForRegistration.UserName,
        };

        var result = await _userManager.CreateAsync(user, userForRegistration.Password!);

        return result;
    }

    public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth)
    {
        _user = await _userManager.FindByNameAsync(userForAuth.UserName!);

        var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password!));
        if (!result)
        {
            //You can use a logger service here to log something like: Authentication failed. Wrong user name or password.
        }

        return result;
    }

    public async Task<TokenDto> CreateToken()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDto(accessToken);
    }

    private SigningCredentials GetSigningCredentials()
    {

        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["YourSecret"];

        var key = Encoding.UTF8.GetBytes(secretKey!);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName!)
            };

        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var tokenOptions = new JwtSecurityToken
        (
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }
}
