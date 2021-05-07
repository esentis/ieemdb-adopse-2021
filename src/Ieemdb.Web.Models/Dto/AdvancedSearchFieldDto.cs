namespace Esentis.Ieemdb.Web.Models.Dto
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public enum OPERATOR
  {
    Eq,
    Gt,
    Lt,
  }

  public class AdvancedSearchFieldDto
  {
    public OPERATOR Operator { get; set; }

    public double Query { get; set; }

  }
}
