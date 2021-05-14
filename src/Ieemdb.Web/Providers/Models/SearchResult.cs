namespace Esentis.Ieemdb.Web.Providers.Models
{
  using System.Collections.Generic;

  public class SearchResult<T>
  {
    public List<T> results { get; set; }

    public int page { get; set; }

    public int total_pages { get; set; }

    public int total_results { get; set; }
  }
}
