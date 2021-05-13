namespace Esentis.Ieemdb.Persistence.Joins
{
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Models;

  public class MovieActor : EmdbKeylessEntity
  {
#nullable disable //  Foreign keys don't need nullability
    public Movie Movie { get; set; }

    public Actor Actor
    {
      get;
      set;
    }

    public string Character { get; set; }
#nullable enable
  }
}
