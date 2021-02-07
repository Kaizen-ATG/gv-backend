public class RedeemOffers
{
    public string Dealtype { get; set; }
    public int PointsRequired { get; set; }
    public string Dealcode { get; set; }
    public string Description { get; set; }

    public RedeemOffers() { }

    public RedeemOffers(string dealtype, int pointsrequired, string dealCode, string description)
    {
        Dealtype = dealtype;
        PointsRequired = pointsrequired;
        Dealcode = dealCode;
        Description = description;
    }
}