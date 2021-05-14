namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Web.Models.Enums;

  using Kritikos.Configuration.Persistence.Contracts.Behavioral;

  public class ServiceBatchingProgress : IEntity<long>, ICreateTimestamped
  {
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public BackgroundServiceName Name { get; set; }

    public int LastProccessedPage { get; set; }

    public int TotalPages { get; set; }
  }
}
