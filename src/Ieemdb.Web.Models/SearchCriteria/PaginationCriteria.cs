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
    public int Page { get; set; } = 1;

    [Range(5, 50)]
    public int ItemsPerPage { get; set; }
  }
}
