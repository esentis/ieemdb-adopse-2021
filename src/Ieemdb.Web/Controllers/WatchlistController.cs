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
    /// Returns watchlist.
    /// </summary>
    /// <returns>Returns list of MovieDto.</returns>
    /// <response code="200">Returns results. </response>
    /// <response code="404">User not found.</response>
    [HttpGet("")]
    public async Task<ActionResult<List<MovieDto>>> GetWatchlist(CancellationToken token = default)
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
        .Project<Movie,MovieDto>(Mapper)
        .ToListAsync(token);

      return Ok(moviesWatchlist);
    }

    /// <summary>
    /// Adds movie on watchlist.
    /// </summary>
    /// <param name="movieId">Movie id.</param>
    /// <response code="200">Returns added successful.</response>
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

      return Ok("Added successfully");
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
  }
}
