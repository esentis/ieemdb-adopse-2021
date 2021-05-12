namespace Esentis.Ieemdb.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models.Enums;
  using Esentis.Ieemdb.Web.Services;

  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Authorization;
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

    public AdminController(
      ILogger<AdminController> logger,
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
    }

    [HttpPost("startDeleteService")]
    public ActionResult InitDeleteService()
    {
      deleteService.Trigger(null);
      return NoContent();
    }

    [HttpPost("startSeeding")]
    public async Task<ActionResult<ServiceBatchingProgress>> InitSeeding(CancellationToken token = default)
    {
      movieSeedService.Trigger(null);
      var status = await Context.ServiceBatchingProgresses.Where(x => x.Name == BackgroundServiceName.MovieSync)
        .OrderByDescending(x => x.CreatedAt)
        .SingleOrDefaultAsync(token);
      return Ok(status);
    }

    [HttpPost("startTokenCleanupService")]
    public ActionResult InitTokenService()
    {
      tokenCleanupService.Trigger(null);
      return NoContent();
    }
  }
}
