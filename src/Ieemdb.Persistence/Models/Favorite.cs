namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Identity;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class Favorite : EemdbEntity<long>
  {
    public IeemdbUser User { get; set; }

    public Movie Movie { get; set; }
  }
}
