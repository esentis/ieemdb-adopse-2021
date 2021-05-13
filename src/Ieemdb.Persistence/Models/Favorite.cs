namespace Esentis.Ieemdb.Persistence.Models
{

  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Identity;

  public class Favorite : EemdbEntity<long>
  {
    public IeemdbUser User { get; set; }

    public Movie Movie { get; set; }
  }
}
