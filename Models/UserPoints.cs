public class UserPoints
{
    public int GreenPoints { get; set; }
    public int CarbonPoints { get; set; }
    public int WeekGP { get; set; }
    public int WeekCP { get; set; }
    public string UserId { get; set; }
    public UserPoints() { }
    public UserPoints(string userid, int greenpoints, int carbonpoints, int weekGP, int weekCP)
    {
        GreenPoints = greenpoints;
        CarbonPoints = carbonpoints;
        WeekGP = weekGP;
        WeekCP = weekCP;
        UserId = userid;
    }
}