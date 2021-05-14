namespace Esentis.Ieemdb.Web.Models.SearchCriteria
{
  using System;
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Class that defines the paging searches.
  /// </summary>
  public class PaginationCriteria
  {
    /// <summary>
    /// Defines the page of the results.
    /// </summary>
    /// <remarks>The starting page is 1.</remarks>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Defines the items per page.
    /// </summary>
    [Range(5, 50)]
    public int ItemsPerPage { get; set; }
  }
}
