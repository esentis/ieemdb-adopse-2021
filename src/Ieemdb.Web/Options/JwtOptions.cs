namespace Esentis.Ieemdb.Web.Options
{
  public class JwtOptions
  {
    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string Key { get; set; }

    public int DurationInMinutes { get; set; }

    public int RefreshTokenDurationInDays { get; set; }
  }
}
