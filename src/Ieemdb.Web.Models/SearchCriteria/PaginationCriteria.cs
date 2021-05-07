namespace Esentis.Ieemdb.Web.Models.SearchCriteria
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class PaginationCriteria
  {
    public int Page { get; set; }

    [Range(1, 100)]
    public int ItemsPerPage { get; set; }
  }
}
