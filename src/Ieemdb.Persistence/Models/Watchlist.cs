namespace Esentis.Ieemdb.Persistence.Models
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Identity;

  public class Watchlist : EemdbEntity<long>
  {
    public string Name { get; set; }

    public IeemdbUser User { get; set; }
  }
}
