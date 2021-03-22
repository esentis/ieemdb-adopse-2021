namespace Esentis.Ieemdb.Web.Models.Dto
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class DirectorDto
  {
    public long Id { get; set; }

    public string Name { get; set; }

    public DateTimeOffset BirthDate { get; set; }

    public string Bio { get; set; }
  }
}
