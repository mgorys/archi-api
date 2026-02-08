using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Archi.Entities;
using Archi.Exceptions;
using Archi.Models;

namespace Archi.Services.Account;

public class AccountService : IAccountService
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly IAccountRepository _accountRepository;

    public AccountService(
        IPasswordHasher<User> passwordHasher, 
        AuthenticationSettings authenticationSettings, 
        IAccountRepository accountRepository
        )
    {
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
        _accountRepository = accountRepository;
    }

    public async Task<string> GenerateJwtAsync(LoginDto dto)
    {
        var user = new User()
        {
            Email = dto.Email,
        };
        var userResponse = await _accountRepository.GetUserAsync(user);

        if (userResponse is null)
        {
            throw new BadRequestException("Invalid username or password");
        }
        if (userResponse.PasswordHash is null)
        {
            throw new BadRequestException("Invalid username or password");
        }
        var result = _passwordHasher.VerifyHashedPassword(userResponse, userResponse.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new BadRequestException("Invalid username or password");
        }

        var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userResponse.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{userResponse.FirstName} {userResponse.LastName}"),
                new Claim(ClaimTypes.Role, $"{userResponse.Role.Name}"),
            };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

        var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
            _authenticationSettings.JwtIssuer,
            claims,
            expires: expires,
            signingCredentials: cred);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public async Task RegisterUserAsync(RegisterUserDto dto)
    {
        var newUser = new User()
        {
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            RoleId = dto.RoleId
        };
        var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
        newUser.PasswordHash = hashedPassword;
        await _accountRepository.RegisterUserAsync(newUser);
    }
}
