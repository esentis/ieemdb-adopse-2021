namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Helpers;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class Genre : EemdbEntity<long>
  {
    public string Name { get; set; }
  }
}
