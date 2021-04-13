namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Abstractions;
  using Esentis.Ieemdb.Persistence.Helpers;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class Movie : IeemdbEntity<long>, IAuditable<Guid>, ISearchable
  {
    private string title = string.Empty;

    private string country = string.Empty;

    public string Title
    {
      get => title;
      set
      {
        title = value;
        NormalizedSearch = title.NormalizeSearch();
      }
    }

    public string NormalizedSearch { get; private set; }

    public TimeSpan Duration { get; set; }

    public string Plot { get; set; }

    public DateTimeOffset ReleaseDate { get; set; }

    public string CountryOrigin
    {
      get => country;
      set
      {
        country = value;
        NormalizedCountry = country.NormalizeSearch();
      }
    }

    public string NormalizedCountry { get; private set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }

    public bool Featured { get; set; }
  }
}
