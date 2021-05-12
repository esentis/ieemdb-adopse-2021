namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Abstractions;
  using Esentis.Ieemdb.Persistence.Helpers;

  public class Actor : EemdbEntity<long>, ISearchable, ISoftDeletable
  {

    private string fullName = string.Empty;

    private string bio = string.Empty;

    public bool IsDeleted { get; set; }

    public string FullName
    {
      get => fullName;
      set
      {
        fullName = value;
        NormalizedFullName = value.NormalizeSearch();
      }
    }

    public string NormalizedFullName { get; private set; } = string.Empty;

    public DateTime? BirthDate { get; set; }

    public string Bio
    {
      get => bio;
      set
      {
        bio = value;
        NormalizedSearch = value.NormalizeSearch();
      }
    }

    public string NormalizedSearch { get; private set; } = string.Empty;

    public bool Featured { get; set; }

    public long TmdbId { get; set; }
  }
}
