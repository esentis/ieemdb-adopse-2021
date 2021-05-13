namespace Esentis.Ieemdb.Web.Models.MovieDBDto
{
  public class Rootobject
  {
    public int page { get; set; }

    public MovieDB[] results { get; set; }

    public int total_results { get; set; }

    public int total_pages { get; set; }
  }

  public class MovieDB
  {
    public string overview { get; set; }
    public long id { get; set; }
    public string title { get; set; }

  }
}
