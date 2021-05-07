namespace Esentis.Ieemdb.Web.Models.Dto
{
  using System;

  public class WriterDto
  {
    public long Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTimeOffset BirthDate { get; set; }

    public string Bio { get; set; }
  }
}
