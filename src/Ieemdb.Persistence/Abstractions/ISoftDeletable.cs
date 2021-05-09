namespace Esentis.Ieemdb.Persistence.Abstractions
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public interface ISoftDeletable
  {
    public bool IsDeleted { get; set; }
  }
}
