namespace Esentis.Ieemdb.Persistence.Identity
{
  using System;

  using Esentis.Ieemdb.Persistence.Helpers;

  public class Device : EemdbEntity<long>
  {
    public string Name { get; set; }

    public IeemdbUser User { get; set; }

    public Guid RefreshToken { get; set; }
  }
}
