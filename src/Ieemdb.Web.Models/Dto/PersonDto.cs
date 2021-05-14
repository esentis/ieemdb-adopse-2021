namespace Esentis.Ieemdb.Web.Models.Dto
{
  using System;

  public class PersonDto
  {
    public long Id { get; set; }

    public string FullName { get; set; }

    public string Bio { get; set; }

    public string? Image { get; set; }

    public DateTime? BirthDay { get; set; }

    public DateTime? DeathDay { get; set; }

    public string KnownFor { get; set; }
  }
}
