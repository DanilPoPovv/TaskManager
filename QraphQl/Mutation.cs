using WebApplication1.Requests;
using WebApplication1.Services;

namespace WebApplication1.QraphQl
{
    public class Mutation 
    {
        public async Task<User> UpdateUser(string userId, UserUpdateRequest request,[Service] IUserService userService)
        {
            return await userService.Update(userId,request);
        }
        public async Task<UserView> Register(RegisterRequest request, [Service] IUserService userService)
        {
            return await userService.Register(request);
        }
    }
}
