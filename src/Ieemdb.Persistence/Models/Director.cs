namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Abstractions;
  using Esentis.Ieemdb.Persistence.Helpers;

  public class Director : EemdbEntity<long>, ISearchable
  {
    private string fName = string.Empty;

    private string lName = string.Empty;

    public string FirstName
    {
      get => fName;
      set
      {
        fName = value;
        NormalizedSearch = value.NormalizeSearch();
      }
    }

    public string LastName
    {
      get => lName;
      set
      {
        lName = value;
        NormalizedSearch = value.NormalizeSearch();
      }
    }

    public string NormalizedSearch { get; private set; } = string.Empty;

    public DateTimeOffset BirthDate { get; set; }

    public string Bio { get; set; } = string.Empty;

    public bool Featured { get; set; }
  }
}
