using Archi.Entities;
using Archi.Models;

namespace Archi.Services.Account
{
    public interface IAccountRepository
    {
        Task RegisterUserAsync(User user);
        Task<User?> GetUserAsync(User dto);
    }
}