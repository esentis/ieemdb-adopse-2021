namespace Esentis.Ieemdb.Persistence.Models
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence.Abstractions;
  using Esentis.Ieemdb.Persistence.Helpers;

  using Kritikos.Configuration.Persistence.Contracts.Behavioral;

  public class Country : EemdbEntity<long>, ISearchable, ISoftDeletable
  {
    private string cname = string.Empty;

    public string Name
    {
      get => cname;
      set
      {
        cname = value;
        NormalizedSearch = cname.NormalizeSearch();
      }
    }

    public bool IsDeleted { get; set; }

    public string NormalizedSearch { get; private set; }
  }
}
