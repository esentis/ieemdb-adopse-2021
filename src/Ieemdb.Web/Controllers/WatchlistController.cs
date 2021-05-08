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
  }
}
