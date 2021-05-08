namespace Esentis.Ieemdb.Web.Models.Dto
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class AddMovieDto
  {
    public string Title { get; set; }

    public double DurationInMinutes { get; set; }

    public string Plot { get; set; }

    public string TrailerUrl { get; set; }

    public DateTimeOffset ReleaseDate { get; set; }

    public List<long> ActorIds { get; set; }

    public List<long> CountryIds { get; set; }

    public List<long> DirectorIds { get; set; }

    public List<long> GenreIds { get; set; }

    public List<long> WriterIds { get; set; }

    public string PosterUrl { get; set; }

    public List<Uri> ScreenshotUrls { get; set; }
  }
}
