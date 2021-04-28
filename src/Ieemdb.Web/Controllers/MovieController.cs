namespace Esentis.Ieemdb.Web.Controllers
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Bogus;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models;

  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Cors;
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

      // We calculate how many movies are in database.
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

    [HttpGet()]
    public async Task<ActionResult<ICollection<MovieDto>>> GetAllMovies(
      [Range(1, 100)] int itemsPerPage = 20,
      int page = 1)
    {
      var toSkip = itemsPerPage * (page - 1);

      var movies = await Context.Movies.Include(x => x.MovieActors).ToListAsync();

      return Ok(movies);
    }

    /// <summary>
    /// This controller gets the list of featured movies.
    /// </summary>
    /// <param name="itemsPerPage">Define how many items shall be returned. </param>
    /// <param name="page">Choose which page of the results shall be returned.</param>
    /// <returns></returns>
    [HttpGet("featured")]
    public async Task<ActionResult<ICollection<MovieDto>>> GetAllFeaturedMovies(int itemsPerPage = 20, int page = 1)
      {
      var toSkip = itemsPerPage * (page - 1);

      var moviesQuery = Context.Movies
        .TagWith("Retrieving all featured movies")
        .Where(x => x.Featured.Equals(true))
        .OrderBy(x => x.Id);

      var totalMovies = await moviesQuery.CountAsync();

      if (page > ((totalMovies / itemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      var pagedMovies = await moviesQuery
        .Skip(toSkip)
        .Take(itemsPerPage)
        .ToListAsync();

      var result = new PagedResult<MovieDto>
      {
        Results = pagedMovies.Select(x => Mapper.Map<Movie, MovieDto>(x)).ToList(),
        Page = page,
        TotalPages = (totalMovies / itemsPerPage) + 1,
        TotalElements = totalMovies,
      };

      return Ok(result);
    }

    /// <summary>
    /// This controller sets the list of featured movies.
    /// </summary>
    /// <param name="featuredIdList"></param>
    /// <response code="200">All Kateuxein. </response>
    /// <response code="404">Couldn't match all id's to movies. </response>
    [HttpPost("featured")]
    public async Task<ActionResult<List<MovieDto>>> AddFeaturedMovie([FromBody] long[] featuredIdList, CancellationToken cancellationToken = default)
    {
      var movies = await Context.Movies.Where(m => featuredIdList.Contains(m.Id)).ToListAsync(cancellationToken);
      var temp = featuredIdList.Where(x => movies.All(y => y.Id != x)).ToArray();

      if (temp.Any())
      {
        return NotFound($"Couldn't find results for the given id's {string.Join(", ", temp)}");
      }

      foreach (var item in movies)
      {
        item.Featured = true;
      }

      await Context.SaveChangesAsync(cancellationToken);

      return Ok(movies.Select(x => Mapper.Map<Movie, MovieDto>(x)));
    }

    /// <summary>
    /// This controller sets the list of featured movies.
    /// </summary>
    /// <param name="featuredIdList"></param>
    /// <returns>No Content.</returns>
    [HttpPost("unfeatured")]
    public async Task<ActionResult> RemoveFeaturedMovie([FromBody] List<long> UnfeaturedIdList)
    {
      Context.Movies.Where(m => UnfeaturedIdList.Contains(m.Id)).ToList().ForEach(mv => mv.Featured = false);

      await Context.SaveChangesAsync();

      return NoContent();
    }
  }
}
