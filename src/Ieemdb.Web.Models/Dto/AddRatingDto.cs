namespace Esentis.Ieemdb.Web.Models.Dto
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class AddRatingDto
  {
    public long MovieId { get; set; }

    public long Rate { get; set; }

    public string Review { get; set; }
  }
}
