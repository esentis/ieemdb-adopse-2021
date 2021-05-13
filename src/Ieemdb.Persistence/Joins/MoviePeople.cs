namespace Esentis.Ieemdb.Persistence.Joins
{

  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Models;

  public class MoviePerson : EmdbKeylessEntity
  {
    public Person Person { get; set; }

    public Movie Movie { get; set; }
  }
}
