namespace Esentis.Ieemdb.Web.Providers.Models
{
  public class DetailedMovie
  {
    public Genre[] genres { get; set; }
    public int id { get; set; }
    public string overview { get; set; }
    public object poster_path { get; set; }
    public Production_Countries[] production_countries { get; set; }
    public string? release_date { get; set; }
    public int? runtime { get; set; }
    public string title { get; set; }
  }

  public class Production_Countries
  {
    public string iso_3166_1 { get; set; }
  }
}
