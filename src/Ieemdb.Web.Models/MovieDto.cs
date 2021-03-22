namespace Esentis.Ieemdb.Web.Models
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Web.Models.Dto;

  public class MovieDto
  {
    public string Title { get; set; } = string.Empty;

    public ActorDto Actor { get; set; }
  }
}
