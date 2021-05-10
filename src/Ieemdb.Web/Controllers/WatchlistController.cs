namespace Esentis.Ieemdb.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Identity;
  using Esentis.Ieemdb.Persistence.Joins;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models;
  using Esentis.Ieemdb.Web.Models.Dto;
  using Esentis.Ieemdb.Web.Models.SearchCriteria;

  using Kritikos.Extensions.Linq;
  using Kritikos.PureMap;
  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Cors;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  [Route("api/watchlist")]
  public class WatchlistController : BaseController<WatchlistController>
  {
    private readonly UserManager<IeemdbUser> userManager;

    public WatchlistController(ILogger<WatchlistController> logger, IeemdbDbContext ctx, IPureMapper mapper, UserManager<IeemdbUser> userManager)
      : base(logger, ctx, mapper)
    {
      this.userManager = userManager;
    }

    /// <summary>
    /// Returns watchlist.
    /// </summary>
    /// <returns>Returns list of MovieDto.</returns>
    /// <response code="200">Returns results. </response>
    /// <response code="400">Page doesn't exist. </response>
    /// <response code="402">Wrong operator </response>
    [HttpGet("")]
    public async Task<ActionResult<List<MovieDto>>> GetWatchlist(CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();

      if (userId == "00000000-0000-0000-0000-000000000000")
      {
        return BadRequest("Something went wrong.");
      }

      var user = await userManager.FindByIdAsync(userId);

      if (user == null)
      {
        return BadRequest("Something went wrong.");
      }

      var watchlistsId = await Context.Watchlists.Where(w => w.User == user)
        .Select(x => x.Movie)
        .Select(x => x.Id)
        .ToListAsync(token);

      var moviesWatchlist = await Context.Movies.Include(x => x.MovieActors)
        .ThenInclude(x => x.Actor)
        .Include(x => x.MovieDirectors)
        .ThenInclude(x => x.Director)
        .Include(x => x.MovieWriters)
        .ThenInclude(x => x.Writer)
        .Include(x => x.MovieGenres)
        .ThenInclude(x => x.Genre)
        .Include(x => x.MovieCountries)
        .ThenInclude(x => x.Country)
        .Where(mv => watchlistsId.Contains(mv.Id))
        .ToListAsync(token);

      return Ok(moviesWatchlist.Select(fm => Mapper.Map<Movie, MovieDto>(fm, "complete")));
    }

    /// <summary>
    /// Adds movie on watchlist.
    /// </summary>
    /// <param name="movieId">Movie id.</param>
    /// <response code="200">Returns added successful.</response>
    /// <response code="400">Page doesn't exist.</response>
    /// <response code="404">Not found given items.</response>
    /// <returns>Ok.</returns>
    [HttpPost("")]
    public async Task<ActionResult> AddMovieToWatchlist(long movieId, CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();

      if (userId == "00000000-0000-0000-0000-000000000000")
      {
        return BadRequest("Something went wrong.");
      }

      var user = await userManager.FindByIdAsync(userId);

      if (user == null)
      {
        return BadRequest("Something went wrong.");
      }

      var movie = await Context.Movies.FirstOrDefaultAsync(x => x.Id == movieId, token);

      if (movie == null)
      {
        return NotFound("Movie not found.");
      }

      var iswatchlist = await Context.Watchlists.AnyAsync(x => x.Movie == movie && x.User == user, token);

      if (iswatchlist)
      {
        return Conflict("Movie is already in watchlist");
      }

      var movieWatchlist = new Watchlist
      {
        Movie = movie,
        User = user,
      };

      Context.Watchlists.Add(movieWatchlist);

      await Context.SaveChangesAsync();

      return Ok("Added successfully");
    }

    /// <summary>
    /// Removes a movie from watchlist.
    /// </summary>
    /// <param name="movieId">Movie's Id.</param>
    /// <returns>List of MovieDto/>.</returns>
    [HttpDelete("")]
    public async Task<ActionResult<ICollection<MovieDto>>> RemoveWatchlist( long movieId, CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();

      if (userId == "00000000-0000-0000-0000-000000000000")
      {
        return BadRequest("Something went wrong.");
      }

      var user = await userManager.FindByIdAsync(userId);

      if (user == null)
      {
        return BadRequest("Something went wrong.");
      }

      var movie = await Context.Watchlists.SingleOrDefaultAsync(w => w.User == user && w.Movie.Id == movieId, token);

      if (movie == null)
      {
        return NotFound("Movie not found");
      }

      Context.Watchlists.Remove(movie);
      await Context.SaveChangesAsync();

      return NoContent();
    }
  }
}
