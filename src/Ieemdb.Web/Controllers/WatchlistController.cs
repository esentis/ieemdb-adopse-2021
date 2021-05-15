namespace Esentis.Ieemdb.Web.Controllers
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Identity;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models;
  using Esentis.Ieemdb.Web.Models.Dto;

  using Kritikos.PureMap;
  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  [Authorize]
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
    /// Returns watchlist movies.
    /// </summary>
    /// <returns>Returns list of MovieDto.</returns>
    /// <response code="200">Returns results. </response>
    /// <response code="404">User not found.</response>
    [HttpGet("")]
    public async Task<ActionResult<List<MovieDto>>> GetMovieWatchlists(CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();

      var user = await userManager.FindByIdAsync(userId);

      if (user == null)
      {
        return NotFound("User not found.");
      }

      var watchlistsId = await Context.Watchlists.Where(w => w.User == user)
        .Select(x => x.Movie)
        .Select(x => x.Id)
        .ToListAsync(token);

      var moviesWatchlist = await Context.Movies.Include(x => x.People)
        .ThenInclude(x => x.Person)
        .Where(mv => watchlistsId.Contains(mv.Id))
        .Project<Movie,MovieMinimalDto>(Mapper)
        .ToListAsync(token);

      return Ok(moviesWatchlist);
    }

    /// <summary>
    /// Adds movie on watchlist.
    /// </summary>
    /// <param name="movieId">Movie id.</param>
    /// <response code="204">Added successful.</response>
    /// <response code="404">User not found. Movie not found.</response>
    /// <response code="409">Movie is already in watchlist.</response>
    /// <returns>Ok.</returns>
    [HttpPost("")]
    public async Task<ActionResult> AddMovieToWatchlist(long movieId, CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();

      var user = await userManager.FindByIdAsync(userId);

      if (user == null)
      {
        return NotFound("User not found.");
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

      await Context.SaveChangesAsync(token);

      return NoContent();
    }

    /// <summary>
    /// Removes a movie from watchlist.
    /// </summary>
    /// <param name="movieId">Movie's Id.</param>
    /// <response code="204">Successfully deleted movie from watchlist.</response>
    /// <response code="404">User not found. Movie not found.</response>
    /// <returns>List of MovieDto/>.</returns>
    [HttpDelete("")]
    public async Task<ActionResult<ICollection<MovieDto>>> RemoveWatchlist( long movieId, CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();

      var user = await userManager.FindByIdAsync(userId);

      if (user == null)
      {
        return NotFound("User not found.");
      }

      var movie = await Context.Watchlists.SingleOrDefaultAsync(w => w.User == user && w.Movie.Id == movieId, token);

      if (movie == null)
      {
        return NotFound("Movie not found");
      }

      Context.Watchlists.Remove(movie);
      await Context.SaveChangesAsync(token);

      return NoContent();
    }

    /// <summary>
    /// Checks if a movie is in watchlist.
    /// </summary>
    /// <param name="movieId">Movie's unique ID. </param>
    /// <response code="200">Returns True or False.</response>
    /// <response code="400">Something went wrong. </response>
    /// <returns>True or False.</returns>
    [HttpPost("check")]
    public async Task<ActionResult<WatchlistDto>> CheckWatchlist(long movieId, CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();
      var user = await userManager.FindByIdAsync(userId);

      if (user == null)
      {
        return BadRequest("Something went wrong.");
      }

      var inList = await Context.Watchlists.AnyAsync(x => x.Movie.Id == movieId && x.User.Id == user.Id,token);

      return Ok(inList);
    }
  }
}
