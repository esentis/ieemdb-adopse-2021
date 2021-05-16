namespace Esentis.Ieemdb.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Identity;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models.Enums;
  using Esentis.Ieemdb.Web.Services;

  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;

  [Authorize(Roles = RoleNames.Administrator)]
  [Microsoft.AspNetCore.Components.Route("api/admin")]
  public class AdminController : BaseController<AdminController>
  {
    private readonly DeletedCleanupService deleteService;
    private readonly MovieSyncingService movieSeedService;
    private readonly RefreshTokenCleanupService tokenCleanupService;
    private readonly UserManager<IeemdbUser> userManager;

    public AdminController(
      ILogger<AdminController> logger,
      UserManager<IeemdbUser> userManager,
      IeemdbDbContext ctx,
      IPureMapper mapper,
      IEnumerable<IHostedService> hostedServices)
      : base(logger, ctx, mapper)
    {
      deleteService = hostedServices.OfType<DeletedCleanupService>().SingleOrDefault()
                      ?? throw new InvalidOperationException(
                        $"Could not locate an instance of the service {nameof(DeletedCleanupService)}");
      movieSeedService = hostedServices.OfType<MovieSyncingService>().SingleOrDefault()
                         ?? throw new InvalidOperationException(
                           $"Could not locate an instance of the service {nameof(MovieSyncingService)}");
      tokenCleanupService = hostedServices.OfType<RefreshTokenCleanupService>().SingleOrDefault()
                            ?? throw new InvalidOperationException(
                              $"Could not locate an instance of the service {nameof(RefreshTokenCleanupService)}");
      this.userManager = userManager;
    }

    [HttpPost("startDeleteService")]
    public ActionResult InitDeleteService()
    {
      deleteService.Trigger(null);
      return NoContent();
    }

    /// <summary>
    /// Starts movie syncing service.
    /// </summary>
    /// <response code="200">Service started.</response>
    /// <returns>All syncing services.</returns>
    [HttpPost("startSyncing")]
    public async Task<ActionResult<ServiceBatchingProgress>> InitSeeding(CancellationToken token = default)
    {
      movieSeedService.Trigger(null);
      var status = await Context.ServiceBatchingProgresses
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync(token);
      return Ok(status);
    }

    /// <summary>
    /// Returns the sync services' status.
    /// </summary>
    /// <response code="200">Returns services.</response>
    /// <returns>List of <see cref="ServiceBatchingProgress"/>.</returns>
    [HttpGet("syncStatus")]
    public async Task<ActionResult<ServiceBatchingProgress>> GetSyncStatus(CancellationToken token = default)
    {
      var status = await Context.ServiceBatchingProgresses
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync(token);
      return Ok(status);
    }

    /// <summary>
    /// Updates a sync service.
    /// </summary>
    /// <param name="id">Service unique ID.</param>
    /// <param name="page">Last proccessed page.</param>
    /// <param name="service">Service enum.</param>
    /// <response code="200">Returns updated service.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Batch service not found.</response>
    /// <returns>Update <see cref="ServiceBatchingProgress"/>.</returns>
    [HttpPut("updateSyncService")]
    public async Task<ActionResult<ServiceBatchingProgress>> EditSyncService(long id, int page,
      BackgroundServiceName service, CancellationToken token = default)
    {
      var status = await Context.ServiceBatchingProgresses.Where(x => x.Name == service && x.Id == id)
        .OrderByDescending(x => x.CreatedAt)
        .SingleOrDefaultAsync(token);

      if (status == null)
      {
        return NotFound("Sync service not found.");
      }

      status.LastProccessedPage = page;
      await Context.SaveChangesAsync();
      return Ok(status);
    }

    [HttpPost("startTokenCleanupService")]
    public ActionResult InitTokenService()
    {
      tokenCleanupService.Trigger(null);
      return NoContent();
    }

    /// <summary>
    /// Deletes all movie sync services.
    /// </summary>
    /// <response code="204">All services deleted.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Batch service not found.</response>
    /// <returns>No content.</returns>
    [HttpDelete("cleanSyncService")]
    public async Task<ActionResult<ServiceBatchingProgress>> CleanSyncService(CancellationToken token = default)
    {
      var movieServices = await Context.ServiceBatchingProgresses.Where(x =>
          x.Name == BackgroundServiceName.PopularMovieSync || x.Name == BackgroundServiceName.TopRatedMovieSync)
        .ToListAsync(token);
      Context.ServiceBatchingProgresses.RemoveRange(movieServices);

      await Context.SaveChangesAsync();
      return NoContent();
    }

    /// <summary>
    /// Returns all users.
    /// </summary>
    /// <response code="204">Returns users.</response>
    /// <response code="401">Unauthorized.</response>
    /// <returns>No content.</returns>
    [HttpGet("getUsers")]
    public async Task<ActionResult<ServiceBatchingProgress>> GetAllUsers(CancellationToken token = default)
    {
      var users = await Context.Users
        .ToListAsync(token);

      await Context.SaveChangesAsync();
      return Ok(users);
    }

    /// <summary>
    /// Removes a user.
    /// </summary>
    /// <response code="204">Removes user.</response>
    /// <response code="400">User has conflicts.</response>
    /// <response code="401">Unauthorized.</response>
    /// <returns>No content.</returns>
    [HttpDelete("deleteUser")]
    public async Task<ActionResult<ServiceBatchingProgress>> RemoveUser(string id, CancellationToken token = default)
    {
      var user = await Context.Users.SingleOrDefaultAsync(x => x.Id == Guid.Parse(id), token);

      var devicesToDelete = await Context.Devices.Where(d => d.User == user).ToListAsync(token);
      var ratingsToDelete = await Context.Ratings.Where(r => r.User == user).ToListAsync(token);
      var watchlistsToDelete = await Context.Watchlists.Where(w => w.User == user).ToListAsync(token);
      var favoritesToDelete = await Context.Favorites.Where(f => f.User == user).ToListAsync(token);

      Context.Devices.RemoveRange(devicesToDelete);
      Context.Ratings.RemoveRange(ratingsToDelete);
      Context.Watchlists.RemoveRange(watchlistsToDelete);
      Context.Favorites.RemoveRange(favoritesToDelete);

      var result = await userManager.DeleteAsync(user);
      if (!result.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      await Context.SaveChangesAsync(token);
      return NoContent();
    }
  }
}
