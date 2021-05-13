namespace Esentis.Ieemdb.Web.Models.Dto
{
  public enum OPERATOR
  {
    Eq,
    Gt,
    Lt,
  }

  public class AdvancedSearchFieldDto
  {
    public OPERATOR Operator { get; set; }

    public double Query { get; set; }

  }
}
