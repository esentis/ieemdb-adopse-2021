namespace Esentis.Ieemdb.Web.Models.Dto
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

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
