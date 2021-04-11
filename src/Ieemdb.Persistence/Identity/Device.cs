namespace Esentis.Ieemdb.Persistence.Identity
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  using Kritikos.Configuration.Persistence.Base;

  public class Device : Entity<long>
  {
    public string Name { get; set; }

    public IeemdbUser User { get; set; }

    public Guid RefreshToken { get; set; }
  }
}
