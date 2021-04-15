namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Helpers;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class Image : EemdbEntity<long>
  {
    public string Url { get; set; }
  }
}
