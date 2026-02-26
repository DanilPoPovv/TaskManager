using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Authorization;
using WebApplication1.EntityFramework;
using WebApplication1.Requests;
using WebApplication1.Views;

namespace WebApplication1.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        public UserService(UserManager<User> UserManager, SignInManager<User> signInManager, JwtTokenGenerator jwtTokenGen)
        {
            _userManager = UserManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGen;
        }
        public async Task<UserView> Register(RegisterRequest request)
        {
            if (await CheckUserNameRegistered(request.UserName))
            {
                throw new Exception("User with this name is already registered");
            }
            var user = await AddUserToDatabase(request);

            return new UserView(user);
        }
        public async Task<AuthorizeView> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                throw new Exception($"No users with Name - \"{request.UserName}\" in system");
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                throw new Exception("Incorrect password");

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            var token = _jwtTokenGenerator.GenerateJwtToken(user, role);

            return new AuthorizeView() { Token = token };

        }
        public async Task<bool> DeleteUser(DeleteUserRequest request)
        {
            var result = await DeleteUserFromDatabase(request.UserId);
            return result;
        }

        public async Task<User> Update(string userId, UserUpdateRequest request)
        {
            var user = await GetUserDatabase(userId);
            user = UpdateUserFields(user, request);
            if (!string.IsNullOrEmpty(request.NewPassword))
                await UpdateUserPassword(user, request.NewPassword, request.OldPassword);
            await _userManager.UpdateAsync(user);
            return user;
        }
        private async Task<bool> DeleteUserFromDatabase(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new Exception("User not found");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            return result.Succeeded;
        }
        private async Task<bool> CheckUserNameRegistered(string UserName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == UserName);
            return user != null;
        }
        private async Task<User> AddUserToDatabase(RegisterRequest request)
        {
            var user = new User { UserName = request.UserName, SecondName = request.SecondName };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            }
            await _userManager.AddToRoleAsync(user, request.IsUserAdmin ? "Admin" : "User");
            return user;
        }
        private async Task<User> GetUserDatabase(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");
            return user;
        }
        private User UpdateUserFields(User user, UserUpdateRequest request)
        {
            user.UserName = request.Name ?? user.UserName;
            user.SecondName = request.SecondName ?? user.SecondName;
            return user;
        }
        private async Task UpdateUserPassword(User user, string oldPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
                throw new Exception(string.Join(';', result.Errors.Select(x => x.Description)));
        }
    }
}
