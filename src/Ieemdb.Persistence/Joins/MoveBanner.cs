namespace Esentis.Ieemdb.Persistence.Joins
{
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Models;

  public class MovieBanner : EmdbKeylessEntity
  {
#nullable disable //  Foreign keys don't need nullability
    public Movie Movie { get; set; }

    public Image Banner { get; set; }
#nullable enable
  }
}
