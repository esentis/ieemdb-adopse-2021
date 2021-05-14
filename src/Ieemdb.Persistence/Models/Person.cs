namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Abstractions;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Web.Models.Enums;

  public class Person : EemdbEntity<long>, ISearchable
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

    public long Id { get; set; }

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

    public string? Image { get; set; }

    public DateTime? BirthDay { get; set; }

    public DateTime? DeathDay { get; set; }

    public DepartmentEnums KnownFor { get; set; }

    public long TmdbId { get; set; }
  }
}
