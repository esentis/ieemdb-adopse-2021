namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class MoviePoster : IAuditable<Guid>
  {
    public Movie Movie { get; set; }

    public Image Poster { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }
  }
}
