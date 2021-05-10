namespace Esentis.Ieemdb.Web.Models.Dto
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class ChangePasswordDto
  {
    public string OldPassword { get; set; }

    public string NewPassword { get; set; }
  }
}
