namespace Esentis.Ieemdb.Web.Models.Dto
{
  using System;

  public class ActorDto
  {
    public long Id { get; set; }

    public string Name { get; set; }

    public DateTimeOffset BirthDate { get; set; }

    public string Bio { get; set; }
  }
}