public class UserView
{
    public UserView(User user)
    {
        Name = user.UserName;
        SecondName = user.SecondName ?? "";
        Email = user.Email ?? "Не указана";
    }

    public string Name { get; set; }
    public string SecondName { get; set; }
    public string Email { get; set; }

    public string FullName => $"{Name} {SecondName}".Trim();
}