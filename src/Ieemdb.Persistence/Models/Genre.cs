namespace Esentis.Ieemdb.Persistence.Models
{
  using Esentis.Ieemdb.Persistence.Helpers;

  using Kritikos.Configuration.Persistence.Contracts.Behavioral;

  public class Genre : EemdbEntity<long>, ISoftDeletable
  {
    public string Name { get; set; }

    public bool IsDeleted { get; set; }

    public long TmdbId { get; set; }
  }
}
