namespace Esentis.Ieemdb.Persistence.Abstractions
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public interface ISearchable
  {
    string NormalizedSearch { get; }
  }
}
