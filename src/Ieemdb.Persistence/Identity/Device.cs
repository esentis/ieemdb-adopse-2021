namespace Esentis.Ieemdb.Persistence.Identity
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence.Helpers;

  using Kritikos.Configuration.Persistence.Base;

  public class Device : IeemdbEntity<long>
  {
    public string Name { get; set; }

    public IeemdbUser User { get; set; }

    public Guid RefreshToken { get; set; }
  }
}
