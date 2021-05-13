namespace Esentis.Ieemdb.Web.Models.Dto
{
  using System;

  public class ActorDto
  {
    public long Id { get; set; }

    public string FullName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string Bio { get; set; }
  }
}
