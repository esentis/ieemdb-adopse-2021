namespace Esentis.Ieemdb.Web.Providers.Models
{
  public class MovieCast
  {
    public long id { get; set; }
    public Cast[] cast { get; set; }
  }

  public class Cast
  {
    public long id { get; set; }

    public string name { get; set; }

    public string original_name { get; set; }

    public string character { get; set; }

  }

}
