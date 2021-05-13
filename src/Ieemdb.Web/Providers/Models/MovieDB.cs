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
    public object poster_path { get; set; }
    public string overview { get; set; }
    public string release_date { get; set; }
    public int[] genre_ids { get; set; }
    public long id { get; set; }
    public string original_title { get; set; }
    public string title { get; set; }
    public object backdrop_path { get; set; }
    public float popularity { get; set; }
    public bool video { get; set; }
  }
}
