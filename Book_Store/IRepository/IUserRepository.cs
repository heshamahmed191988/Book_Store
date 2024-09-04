using Book_Store.Models;
using Microsoft.AspNetCore.Identity;

namespace Book_Store.IRepository
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterAsync(User user, string password);
        Task<User> LoginAsync(string username, string password);
        Task<SignInResult> PasswordSignInAsync(string username, string password, bool rememberMe);
        Task LogoutAsync();
    }
}
