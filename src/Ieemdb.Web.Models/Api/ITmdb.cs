namespace Esentis.Ieemdb.Web.Models.Api
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Web.Models.MovieDBDto;

  using Refit;

  public interface ITmdb
  {
    [Get("/discover/movie")]
    Task<Rootobject> DiscoverMovies();
  }
}
