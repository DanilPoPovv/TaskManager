using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

public class User : IdentityUser
{
    public string SecondName { get; set; }
    [JsonIgnore]
    public IList<AppTask> Tasks { get; set; }
}