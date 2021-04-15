namespace Esentis.Ieemdb.Persistence.Joins
{
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Models;

  public class MovieDirector : EmdbKeylessEntity
  {
    public Movie Movie { get; set; }

    public Director Director { get; set; }
  }
}
