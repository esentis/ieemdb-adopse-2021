namespace Esentis.Ieemdb.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Bogus;

  using EllipticCurve.Utils;

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

  [Route("api/movie")]
  public class WatchlistController : BaseController<WatchlistController>
  {
    private readonly UserManager<IeemdbUser> userManager;

    public WatchlistController(ILogger<WatchlistController> logger, IeemdbDbContext ctx, IPureMapper mapper, UserManager<IeemdbUser> userManager)
      : base(logger, ctx, mapper)
    {
      this.userManager = userManager;
    }

    /// <summary>
    /// Returns all user's lists.
    /// </summary>
    /// <returns>All user's lists.</returns>
    /// <response code="200">Returns results. </response>
    /// <response code="400">Page doesn't exist. </response>
    /// <response code="402">Wrong operator </response>
    [HttpGet("usersLists")]
    public async Task<ActionResult<List<Watchlist>>> GetAllWatchlists()
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

      var watchlists = Context.Watchlists.Where(x => x.User == user);

      return Ok(watchlists);
    }

    /// <summary>
    /// Returns a specific user's list.
    /// </summary>
    /// <param name="watchlistId">Watchlist's id.</param>
    /// <returns>Movies from Watchlist .</returns>
    /// <response code="200">Returns results. </response>
    /// <response code="400">Page doesn't exist. </response>
    [HttpGet("list")]
    public async Task<ActionResult<List<MovieWatchlist>>> GetWatchlist(long watchlistId, CancellationToken token = default)
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

      var watchlists = Context.Watchlists.Where(x => x.Id == watchlistId);

      var movieWatchlist = Context.MovieWatchlists.Where(x => x.Watchlist == watchlists);

      return Ok(movieWatchlist);
    }

    /// <summary>
    /// Adds movie on watchlist.
    /// </summary>
    /// <param name="movieId">Movie id.</param>
    /// <param name="watchlistId">Watchlist id.</param>
    /// <response code="200">Returns added successful.</response>
    /// <response code="400">Page doesn't exist.</response>
    /// <response code="404">Not found given items.</response>
    /// <returns>Ok.</returns>
    [HttpPost("addMovieToWatchlist")]
    public async Task<ActionResult> AddMovieToWatchlist(long movieId, long watchlistId, CancellationToken token = default)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState.Values.SelectMany(c => c.Errors));
      }

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

      var watchlist = await Context.Watchlists.FirstOrDefaultAsync(x => x.Id == watchlistId && x.User == user, token);

      if (watchlist == null)
      {
        return NotFound("Watchlist not found.");
      }

      var movieWatchlist = new MovieWatchlist
      {
        Movie = movie,
        Watchlist = watchlist,
      };

      Context.MovieWatchlists.Add(movieWatchlist);

      await Context.SaveChangesAsync();

      return Ok("Added successfully");
    }

    /// <summary>
    /// Adds a new watchlist.
    /// </summary>
    /// <param name="listname">List's name.</param>
    /// <response code="200">Returns added successful.</response>
    /// <response code="400">Page doesn't exist.</response>
    /// <response code="404">Not found given items.</response>
    /// <returns>Ok.</returns>
    [HttpPost("addWatchlist")]
    public async Task<ActionResult> AddWatchlist(string listname, CancellationToken token = default)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState.Values.SelectMany(c => c.Errors));
      }

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

      var watchlistExists = await Context.Watchlists.FirstOrDefaultAsync(x => x.Name == listname && x.User == user, token);

      if (watchlistExists != null)
      {
        return BadRequest("Watchlist with the same name already exists.");
      }

      var watchlist = new Watchlist
      {
        Name = listname,
        User = user,
      };

      Context.Watchlists.Add(watchlist);

      await Context.SaveChangesAsync();

      return Ok("Added successfully");
    }
  }
}
