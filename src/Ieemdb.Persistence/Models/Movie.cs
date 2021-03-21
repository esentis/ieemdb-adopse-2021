namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Base;
  using Esentis.Ieemdb.Persistence.Helpers;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class Movie : Entity<long>, IAuditable<Guid>
  {
    private string title = string.Empty;

    private string country = string.Empty;

    public string Title
    {
      get => title;
      set
      {
        title = value;
        NormalizedTitle = title.NormalizeSearch();
      }
    }

    public string NormalizedTitle { get; private set; }

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
  }
}