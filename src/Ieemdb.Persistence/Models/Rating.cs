namespace Esentis.Ieemdb.Persistence.Models
{

  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Identity;

  public class Rating : EemdbEntity<long>
  {
    public IeemdbUser User { get; set; }

    public double Rate { get; set; }

    public string? Review { get; set; }

    public Movie Movie { get; set; }
  }
}
