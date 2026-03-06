using WebApplication1.EntityFramework;
using WebApplication1.Services;

public class Query
{
    public string Hello() => "Hello GraphQL";
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<User> GetAllUsers([Service] AppDbContext dbContext)
    {
        return dbContext.Users;
    }
    public async Task<string> Ok([Service] IUserService userService)
    {
        await userService.GetAllUsers();
        return "Ok";
    }
}