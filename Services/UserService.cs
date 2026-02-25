using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.EntityFramework;
using WebApplication1.Requests;
using WebApplication1.Views;

namespace WebApplication1.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        public UserService(UserManager<User> UserManager)
        {
            _userManager = UserManager;
        }
        public async Task<UserView> Register(RegisterRequest request)
        {
            if (await CheckUserNameRegistered(request.UserName))
            {
                throw new Exception("User with this name is already registered");
            }
            var user = new User { UserName = request.UserName, SecondName = request.SecondName };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            }
            await _userManager.AddToRoleAsync(user, request.IsUserAdmin ? "Admin" : "User");
            return new UserView(user);
        }
        private async Task<bool> CheckUserNameRegistered(string UserName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == UserName);
            return user != null;
        }
    }
}
