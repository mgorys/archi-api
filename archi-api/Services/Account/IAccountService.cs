using Archi.Models;

namespace Archi.Services.Account
{
    public interface IAccountService
    {
        public Task RegisterUserAsync(RegisterUserDto dto);
        public Task<string> GenerateJwtAsync(LoginDto dto);
    }
}