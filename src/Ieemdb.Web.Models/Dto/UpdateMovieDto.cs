namespace Esentis.Ieemdb.Web.Models.Dto
{
  using System;
  using System.ComponentModel.DataAnnotations;

  public class UpdateMovieDto
  {
    [Required]
    public string Title { get; set; }

    public TimeSpan Duration { get; set; }

    public string Plot { get; set; }

    public string TrailerUrl { get; set; }

    public DateTimeOffset ReleaseDate { get; set; }

  }
}
