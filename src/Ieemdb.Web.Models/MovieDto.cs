namespace Esentis.Ieemdb.Web.Models
{
  using System;
  using System.Collections.Generic;

  using Esentis.Ieemdb.Web.Models.Dto;

  public class MovieDto
  {
    public long Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Plot { get; set; }

    public string TrailerUrl { get; set; }

    public TimeSpan Duration { get; set; }

    public bool Featured { get; set; }

    public double Rating { get; set; }

    public List<GenreDto> Genres { get; set; }

    public DateTimeOffset ReleaseDate { get; set; }

    public string Country { get; set; }

    public List<ActorDto> Actors { get; set; }

    public List<DirectorDto> Directors { get; set; }

    public List<WriterDto> Writers { get; set; }

    public List<ImageDto> Posters { get; set; }

    public List<ImageDto> Banners { get; set; }

    public List<ImageDto> Screenshots { get; set; }
  }
}
