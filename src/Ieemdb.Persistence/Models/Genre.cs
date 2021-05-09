namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Abstractions;
  using Esentis.Ieemdb.Persistence.Helpers;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class Genre : EemdbEntity<long>, ISoftDeletable
  {
    public string Name { get; set; }

    public bool IsDeleted { get; set; }
  }
}
