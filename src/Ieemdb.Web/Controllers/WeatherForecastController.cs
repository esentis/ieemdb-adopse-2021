namespace Esentis.Ieemdb.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using AutoMapper;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models;

  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Logging;

  [Authorize]
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : BaseController<WeatherForecastController>
  {
    private static readonly string[] Summaries = new[]
    {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
    };

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IeemdbDbContext ctx, IPureMapper mapper)
      : base(logger, ctx, mapper)
    {
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
      var rng = new Random();
      return Enumerable.Range(1, 5)
        .Select(index => new WeatherForecast
        {
          Date = DateTime.Now.AddDays(index),
          TemperatureC = rng.Next(-20, 55),
          Summary = Summaries[rng.Next(Summaries.Length)],
        })
        .ToArray();
    }

    [HttpGet("foo")]
    [AllowAnonymous]
    public ActionResult<IEnumerable<MovieDto>> Foo()
    {
      var rng = new Random();
      var forecasts = Enumerable.Range(1, 5)
        .Select(index => new WeatherForecast
        {
          Date = DateTime.Now.AddDays(index),
          TemperatureC = rng.Next(-20, 55),
          Summary = Summaries[rng.Next(Summaries.Length)],
        })
        .ToList();

      var foo = forecasts.Select(x => Mapper.Map<WeatherForecast, MovieDto>(x, "withActor"));
      return Ok(foo);
    }
  }
}
