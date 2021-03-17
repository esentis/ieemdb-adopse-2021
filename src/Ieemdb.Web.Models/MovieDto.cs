namespace Esentis.Ieemdb.Web.Models
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class MovieDto
  {
    public string Title { get; set; } = string.Empty;

    public ActorDto Actor { get; set; }
  }

  public class ActorDto
  {
    public string Name { get; set; } = string.Empty;

    private List<MovieDto> Movies { get; set; } = new();
  }
}
