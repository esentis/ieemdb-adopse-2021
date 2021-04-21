namespace Esentis.Ieemdb.Persistence.Models
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence.Abstractions;
  using Esentis.Ieemdb.Persistence.Helpers;

  public class Country : EemdbEntity<long>, ISearchable
  {
    private string cname = string.Empty;

    public string CountryOrigin
    {
      get => cname;
      set
      {
        cname = value;
        NormalizedSearch = cname.NormalizeSearch();
      }
    }

    public string NormalizedSearch { get; private set; }
  }
}
