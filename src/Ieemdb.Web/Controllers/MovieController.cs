namespace Esentis.Ieemdb.Web.Controllers
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Identity;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models;

  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  [Route("api/movie")]
  public class MovieController : BaseController<MovieController>
  {
    public MovieController(ILogger<MovieController> logger, IeemdbDbContext ctx, IPureMapper mapper)
      : base(logger, ctx, mapper)
    {
    }

    [HttpGet("search")]
    public async Task<ActionResult<ICollection<MovieDto>>> SearchForMovie(
      string query,
      [Range(1, 100)] int itemsPerPage = 20,
      int page = 1)
    {
      var toSkip = itemsPerPage * (page - 1);

      // We prepare the query without executing it.
      var moviesQuery = Context.Movies
        .TagWith($"Searching for movies with {query}")
        .FullTextSearch(query)
        .OrderBy(x => x.Id);

      // We calculate how many actors are in database.
      var totalMovies = await moviesQuery.CountAsync();

      // If page provided doesn't exist we return bad request.
      if (page > ((totalMovies
                   / itemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      // We create the paged query request.
      var pagedMovies = await moviesQuery
        .Skip(toSkip)
        .Take(itemsPerPage)
        .ToListAsync();

      // We create the result, which is paged.
      var result = new PagedResult<MovieDto>
      {
        Results = pagedMovies.Select(x => Mapper.Map<Movie, MovieDto>(x)).ToList(),
        Page = page,
        TotalPages = (totalMovies / itemsPerPage) + 1,
        TotalElements = totalMovies,
      };

      // We return OK and the paged results;
      return Ok(result);
    }
  }
}
