namespace Esentis.Ieemdb.Persistence.Models
{
  using System;
  using System.Collections.Generic;

  using Esentis.Ieemdb.Persistence.Abstractions;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Joins;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class Movie : EemdbEntity<long>, ISearchable, ISoftDeletable
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

    public string NormalizedSearch { get; private set; }

    public double AverageRating { get; set; }

    public DateTimeOffset ReleaseDate { get; set; }

    public bool Featured { get; set; }

    public string PosterUrl { get; set; }

    public bool IsDeleted { get; set; }

    public IReadOnlyCollection<MovieGenre> MovieGenres { get; set; }
      = new List<MovieGenre>(0);

    public IReadOnlyCollection<MovieActor> MovieActors { get; set; }
      = new List<MovieActor>(0);

    public IReadOnlyCollection<MovieDirector> MovieDirectors { get; set; }
      = new List<MovieDirector>(0);

    public IReadOnlyCollection<MovieWriter> MovieWriters { get; set; }
      = new List<MovieWriter>(0);

    public IReadOnlyCollection<MovieCountry> MovieCountries { get; set; }
      = new List<MovieCountry>(0);

    public IReadOnlyCollection<Rating> Ratings { get; set; }
      = new List<Rating>(0);

    public IReadOnlyCollection<Screenshot> Screenshots { get; set; }
      = new List<Screenshot>(0);

  }
}
