namespace Esentis.Ieemdb.Web.Models
{
  using System;
  using System.Collections.Generic;

  using Esentis.Ieemdb.Web.Models.Dto;

  public class MovieMinimalDto
  {
    public long Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Plot { get; set; }

    public double AverageRating { get; set; }

    public TimeSpan Duration { get; set; }

    public bool Featured { get; set; }

    public List<GenreDto> Genres { get; set; }

    public DateTimeOffset? ReleaseDate { get; set; }

    public string PosterUrl { get; set; }


  }
}
