namespace Esentis.Ieemdb.Persistence.Models
{

  using Esentis.Ieemdb.Persistence.Helpers;

  using Kritikos.Configuration.Persistence.Contracts.Behavioral;

  public class Image : EemdbEntity<long>, ISoftDeletable
  {
    public int Id { get; set; }

    public string Url { get; set; }

    public Movie Movie { get; set; }

    public bool IsDeleted { get; set; }
  }
}
