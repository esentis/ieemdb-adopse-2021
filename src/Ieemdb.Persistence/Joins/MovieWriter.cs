namespace Esentis.Ieemdb.Persistence.Joins
{
  using System;

  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Models;

  public class MovieWriter : EmdbKeylessEntity
  {
#nullable disable //  Foreign keys don't need nullability
    public Writer Writer { get; set; }

    public Movie Movie { get; set; }
#nullable enable
  }
}
