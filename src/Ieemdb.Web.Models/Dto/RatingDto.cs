namespace Esentis.Ieemdb.Web.Models.Dto
{
  using System;

  public class RatingDto
  {
    public long Id { get; set; }

    public double Rate { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string MovieName { get; set; }

    public long MovieId { get; set; }

    public string Review { get; set; }

    public string Username { get; set; }
  }
}
