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

public class AccountRepository : IAccountRepository
{
    private readonly ArchiDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;

    public AccountRepository(
        ArchiDbContext context, 
        IPasswordHasher<User> passwordHasher, 
        AuthenticationSettings authenticationSettings
        )
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
    }

    public async Task<User?> GetUserAsync(User dto)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == dto.Email);
    }

    public async Task RegisterUserAsync(User newUser)
    {
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
    }
}
