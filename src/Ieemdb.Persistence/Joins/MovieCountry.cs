namespace Esentis.Ieemdb.Persistence.Joins
{

  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Models;

  public class MovieCountry : EmdbKeylessEntity
  {
#nullable disable //  Foreign keys don't need nullability
    public Movie Movie { get; set; }

    public Country Country { get; set; }
#nullable enable
  }
}
