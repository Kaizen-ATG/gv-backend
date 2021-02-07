public class User
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public int Role { get; set; }
    public User() { }

    public User(string userid, string username, string email, int roleid)
    {
        UserId = userid;
        UserName = username;
        Email = email;
        Role = roleid;
    }
}