namespace Esentis.Ieemdb.Web.Models.SearchCriteria
{
  using System;

  public class MovieSearchCriteria : PaginationCriteria
  {

    public string? TitleCriteria { get; set; }

    public string? PlotCriteria { get; set; }

    public bool? IsFeatured { get; set; }

    public string? Actor { get; set; }

    public string? Director { get; set; }

    public string? Writer { get; set; }

    public double? MaxRating { get; set; }

    public double? MinRating { get; set; }

    public DateTimeOffset? FromYear { get; set; }

    public DateTimeOffset? ToYear { get; set; }

    public double? MinDurationInMinutes { get; set; }

    public double? MaxDurationInMinutes { get; set; }

    public long[] Genres { get; set; } = Array.Empty<long>();
  }
}
