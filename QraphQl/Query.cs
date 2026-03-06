using WebApplication1.Services;

public class Query
{
    public string Hello() => "Hello GraphQL";
    public async Task<List<User>> GetAllUsers([Service] IUserService userService)
    {
        return await userService.GetAllUsers();
    }
    public async Task<string> Ok([Service] IUserService userService)
    {
        await userService.GetAllUsers();
        return "Ok";
    }
}