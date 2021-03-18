namespace Esentis.Ieemdb.Web.Helpers
{
  using System;
  using System.Security.Claims;

  using Esentis.Ieemdb.Persistence;

  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Logging;

  public abstract class BaseController<T> : ControllerBase
  {
    protected BaseController(ILogger<T> logger, IeemdbDbContext ctx, IPureMapper mapper)
    {
      Logger = logger;
      Context = ctx;
      Mapper = mapper;
    }

    protected ILogger<T> Logger { get; init; }

    protected IeemdbDbContext Context { get; init; }

    protected IPureMapper Mapper { get; init; }

    protected Guid RetrieveUserId() =>
      Guid.TryParse(HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var guid)
        ? guid
        : Guid.Empty;
  }
}
