namespace Esentis.Ieemdb.Persistence.Joins
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Models;

  public class MovieWatchlist : EmdbKeylessEntity
  {
#nullable disable
    public Movie Movie { get; set; }

    public Watchlist Watchlist { get; set; }
#nullable enable
  }
}
