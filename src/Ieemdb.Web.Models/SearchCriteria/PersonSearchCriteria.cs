namespace Esentis.Ieemdb.Web.Models.SearchCriteria
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class PersonSearchCriteria : PaginationCriteria
  {
    public string Query { get; set; }
  }
}
