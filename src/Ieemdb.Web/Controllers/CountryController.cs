namespace Esentis.Ieemdb.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Helpers;

  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Logging;

  [Route("api/country")]
  public class CountryController : BaseController<CountryController>
  {
    public CountryController(ILogger<CountryController> logger, IeemdbDbContext ctx, IPureMapper mapper)
      : base(logger, ctx, mapper)
    {
    }

    [HttpPost("")]
    public async Task<ActionResult> AddCountry([FromBody]string name, CancellationToken token = default)
    {
      if (name.Length == 0)
      {
        return BadRequest("Name should not be empty !");
      }

      var country = new Country {Name = name};
      Context.Countries.Add(country);
      await Context.SaveChangesAsync(token);
      return Ok(country);
    }
  }
}
