namespace Esentis.Ieemdb.Web.Models
{
  using Esentis.Ieemdb.Web.Models.Dto;

  public class MovieDto
  {
    public string Title { get; set; } = string.Empty;

    public ActorDto Actor { get; set; }

    public DirectorDto Director { get; set; }
  }
}
