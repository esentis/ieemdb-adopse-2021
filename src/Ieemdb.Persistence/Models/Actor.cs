namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Abstractions;
  using Esentis.Ieemdb.Persistence.Helpers;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class Actor : IeemdbEntity<long>, IAuditable<Guid>, ISearchable
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

    public string NormalizedSearch { get; private set; }

    public DateTimeOffset BirthDate { get; set; }

    public string Bio { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }

    public bool Featured { get; set; }
  }
}
