using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public string SecondName { get; set; }
    
    public IList<AppTask> Tasks { get; set; }
}