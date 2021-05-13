namespace Esentis.Ieemdb.Persistence.Models
{
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Web.Models.Enums;

  using Kritikos.Configuration.Persistence.Contracts.Behavioral;

  public class Video : EemdbEntity<long>, ISoftDeletable
  {
    public long Id { get; set; }

    public bool IsDeleted { get ;set; }

    public string Key { get; set; }

    public string TmdbId { get; set; }

    public string Site { get; set; }

    public VideoTypeEnums Type { get; set; }

    public Movie Movie { get; set; }
  }
}
