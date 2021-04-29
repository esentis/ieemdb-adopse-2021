namespace Esentis.Ieemdb.Persistence.Models
{
  using System;
  using System.Collections.Generic;

  using Esentis.Ieemdb.Persistence.Abstractions;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Joins;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class Movie : EemdbEntity<long>, ISearchable
  {
    private string title = string.Empty;
    private string plot = string.Empty;

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

    public string TrailerUrl { get; set; }

    public TimeSpan Duration { get; set; }

    public string Plot
    {
      get => plot;
      set
      {
        plot = value;
        NormalizedSearch = value.NormalizeSearch();
      }
    }

    public string NormalizedSearch { get; set; }

    public DateTimeOffset ReleaseDate { get; set; }

    public bool Featured { get; set; }

  }
}
