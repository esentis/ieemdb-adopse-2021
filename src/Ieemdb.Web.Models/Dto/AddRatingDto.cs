namespace Esentis.Ieemdb.Web.Models.Dto
{
  public class AddRatingDto
  {
    public long MovieId { get; set; }

    public long Rate { get; set; }

    public string Review { get; set; }
  }
}
