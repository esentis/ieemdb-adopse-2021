namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Abstractions;
  using Esentis.Ieemdb.Persistence.Helpers;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class Writer : EemdbEntity<long>, ISearchableNames
  {
    private string fName = string.Empty;

    private string lName = string.Empty;

    public string FirstName
    {
      get => fName;
      set
      {
        fName = value;
        NormalizedFirstNameSearch = value.NormalizeSearch();
      }
    }

    public string LastName
    {
      get => lName;
      set
      {
        lName = value;
        NormalizedLastNameSearch = value.NormalizeSearch();
      }
    }

    public string NormalizedFirstNameSearch { get; private set; } = string.Empty;

    public string NormalizedLastNameSearch { get; private set; } = string.Empty;

    public DateTimeOffset BirthDate { get; set; }

    public string Bio { get; set; }

    public bool Featured { get; set; }
  }
}
