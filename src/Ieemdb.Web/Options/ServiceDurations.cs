namespace Esentis.Ieemdb.Web.Options
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  public class ServiceDurations
  {
    public int DeleteServiceInMinutes { get; set; }

    public int CleanupTokensInMinutes { get; set; }

    public int DatabaseSeedingInMinutes { get; set; }
  }
}
