namespace Esentis.Ieemdb.Persistence.Joins
{
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Models;

  public class MoviePoster : EmdbKeylessEntity
  {
#nullable disable //  Foreign keys don't need nullability
    public Movie Movie { get; set; }

    public Image Poster { get; set; }
#nullable enable
  }
}
