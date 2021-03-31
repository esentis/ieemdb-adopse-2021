namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class MovieGenre : IAuditable<Guid>
  {
    public Movie Movie { get; set; }

    public Genre Genre { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }
  }
}
