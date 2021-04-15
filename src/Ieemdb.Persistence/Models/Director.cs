namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Abstractions;
  using Esentis.Ieemdb.Persistence.Helpers;

  public class Director : EemdbEntity<long>, ISearchable
  {
    private string name = string.Empty;

    public string Name
    {
      get => name;
      set
      {
        name = value;
        NormalizedSearch = value.NormalizeSearch();
      }
    }

    public string NormalizedSearch { get; private set; } = string.Empty;

    public DateTimeOffset BirthDate { get; set; }

    public string Bio { get; set; } = string.Empty;

    public bool Featured { get; set; }
  }
}
