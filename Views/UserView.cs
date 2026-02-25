namespace WebApplication1.Views
{
    public class UserView
    {
        public UserView(User User)
        {
            Name = User.UserName;
            Email = User.Email ?? "Не указана";
        }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
