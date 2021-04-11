namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Identity;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class Rating : IeemdbEntity<long>, IAuditable<Guid>
  {
    public IeemdbUser User { get; set; }

    public double Rate { get; set; }

    public Movie Movie { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }
  }
}
