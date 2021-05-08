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
    /// <param name="id">Actor's ID.</param>
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
  }
}
