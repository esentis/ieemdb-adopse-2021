namespace Esentis.Ieemdb.Persistence.Models
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Web.Models.Enums;

  using Kritikos.Configuration.Persistence.Contracts.Behavioral;

  using Microsoft.Extensions.Hosting;

  public class ServiceBatchingProgress : IEntity<long>, ICreateTimestamped
  {
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public BackgroundServiceName Name { get; set; }

    public int LastProccessedPage { get; set; }

    public int TotalPages { get; set; }
  }
}
