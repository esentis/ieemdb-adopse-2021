namespace Esentis.Ieemdb.Persistence.Joins
{
  using System;

  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Models;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class MovieGenre : EmdbKeylessEntity
  {
#nullable disable //  Foreign keys don't need nullability
    public Movie Movie { get; set; }

    public Genre Genre { get; set; }
#nullable enable
  }
}
