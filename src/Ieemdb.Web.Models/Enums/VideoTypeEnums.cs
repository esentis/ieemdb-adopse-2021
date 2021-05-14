namespace Esentis.Ieemdb.Web.Models.Enums
{
  using System;

  [Flags]
  public enum VideoTypeEnums
  {
    None = 0,
    Trailer = 1,
    Teaser = Trailer << 1,
    Clip = Teaser << 1,
    Featurette = Clip << 1,
    BehindTheScenes = Featurette << 1,
    Bloopers = BehindTheScenes << 1,
  }
}
