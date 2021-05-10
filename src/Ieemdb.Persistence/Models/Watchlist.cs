namespace Esentis.Ieemdb.Persistence.Models
{

  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Identity;

  public class Watchlist : EemdbEntity<long>
  {
    public Movie Movie { get; set; }

    public IeemdbUser User { get; set; }
  }
}
