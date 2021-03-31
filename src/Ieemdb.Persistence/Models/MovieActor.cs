namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class MovieActor : IAuditable<Guid>
  {
    public Movie Movie { get; set; }

    public Actor Actor { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }
  }
}
