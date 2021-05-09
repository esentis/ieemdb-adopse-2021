namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Abstractions;
  using Esentis.Ieemdb.Persistence.Helpers;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class Writer : EemdbEntity<long>, ISearchable, ISoftDeletable
  {
    private string fName = string.Empty;

    private string lName = string.Empty;

    private string bio = string.Empty;

    public string FirstName
    {
      get => fName;
      set
      {
        fName = value;
        NormalizedFirstName = value.NormalizeSearch();
      }
    }

    public string LastName
    {
      get => lName;
      set
      {
        lName = value;
        NormalizedLastName = value.NormalizeSearch();
      }
    }

    public bool IsDeleted { get; set; }

    public string NormalizedFirstName { get; private set; } = string.Empty;

    public string NormalizedLastName { get; private set; } = string.Empty;

    public DateTimeOffset BirthDate { get; set; }

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
  }
}
