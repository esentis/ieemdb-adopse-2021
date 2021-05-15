namespace Esentis.Ieemdb.Web.Models.Enums
{
  using System;

  [Flags]
  public enum BackgroundServiceName
  {
    None = 0,
    DeleteCleanup = 1,
    RefreshToken = DeleteCleanup << 1,
    PopularMovieSync = RefreshToken << 1,
    TopRatedMovieSync = PopularMovieSync << 1,
  }
}
