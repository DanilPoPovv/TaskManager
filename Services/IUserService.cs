using WebApplication1.Requests;
using WebApplication1.Views;

namespace WebApplication1.Services
{
    public interface IUserService
    {
        Task<UserView> Register(RegisterRequest request);
        Task<AuthorizeView> Login(LoginRequest request);
        Task<bool> DeleteUser(DeleteUserRequest request);
    }
}
