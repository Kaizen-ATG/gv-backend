public class UserDetail
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public int Role { get; set; }
    public int GreenPoints { get; set; }
    public int CarbonPoints { get; set; }
    public int WeekGP { get; set; }
    public int WeekCP { get; set; }
    public UserDetail() { }

    public UserDetail(string userid, string username, int roleid, int greenpoints, int carbonpoints, int weekGP, int weekCP)
    {
        UserId = userid;
        UserName = username;
        Role = roleid;
        GreenPoints = greenpoints;
        CarbonPoints = carbonpoints;
        WeekGP = weekGP;
        WeekCP = weekCP;
    }
}