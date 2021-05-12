namespace Esentis.Ieemdb.Web.Providers.Models
{
  public class GenreResults
  {
    public Genre[] genres { get; set; }
  }

  public class Genre
  {
    public long id { get; set; }
    public string name { get; set; }
  }
}
