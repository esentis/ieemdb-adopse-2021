namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Base;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class MovieWriter : Entity<long>, IAuditable<Guid>
  {
    public Writer Writer { get; set; }

    public Movie Movie { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }
  }
}
