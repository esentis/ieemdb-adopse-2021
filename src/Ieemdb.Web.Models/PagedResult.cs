namespace Esentis.Ieemdb.Web.Models
{
  using System.Collections.Generic;

  public class PagedResult<T>
  {
#pragma warning disable CA2227 // False alarm
    public List<T> Results { get; set; } = new List<T>();
#pragma warning restore CA2227 // Collection properties should be read only

    public int Page { get; set; }

    public int TotalPages { get; set; }

    public int TotalElements { get; set; }
  }
}
