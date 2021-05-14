namespace Esentis.Ieemdb.Web.Models.Enums
{
  using System;

  [Flags]
  public enum DepartmentEnums
  {
    None = 0,
    Acting = 1,
    Directing = Acting << 1,
    Production = Directing << 1,
    Editing = Production << 1,
    Art = Editing << 1,
    Sound = Art << 1,
    Writing = Sound << 1,
  }
}
