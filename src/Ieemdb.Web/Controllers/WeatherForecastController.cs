namespace Esentis.Ieemdb.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using AutoMapper;

  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Models;

  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Logging;

  [Authorize]
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private static readonly string[] Summaries = new[]
    {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
    };

    private readonly ILogger<WeatherForecastController> logger;
    private readonly IPureMapper Mapper;
    public WeatherForecastController(ILogger<WeatherForecastController> logger, IPureMapper mapper)
    {
      this.logger = logger;
      Mapper = mapper;
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
