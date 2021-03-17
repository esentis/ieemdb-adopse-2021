namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Base;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class MovieGenre : Entity<long>, IAuditable<Guid>
  {
    public Movie Movie { get; set; }

    public Genre Genre { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }
  }
}
