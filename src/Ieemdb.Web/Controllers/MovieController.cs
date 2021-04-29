namespace Esentis.Ieemdb.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Bogus;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Joins;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models;
  using Esentis.Ieemdb.Web.Models.Dto;

  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Cors;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  [Route("api/movie")]
  public class MovieController : BaseController<MovieController>
  {

    public MovieController(ILogger<MovieController> logger, IeemdbDbContext ctx, IPureMapper mapper)
      : base(logger, ctx, mapper)
    {
    }

    [HttpGet("search")]
    public async Task<ActionResult<ICollection<MovieDto>>> SearchForMovie(
      string query,
      [Range(1, 100)] int itemsPerPage = 20,
      int page = 1)
    {
      var toSkip = itemsPerPage * (page - 1);

      // We prepare the query without executing it.
      var moviesQuery = Context.Movies
        .TagWith($"Searching for movies with {query}")
        .Where(x => EF.Functions.ToTsVector("english", x.NormalizedSearch).Matches(query) || EF.Functions.ToTsVector("english", x.NormalizedTitle).Matches(query))

        .OrderBy(x => x.Id);

      // We calculate how many movies are in database.
      var totalMovies = await moviesQuery.CountAsync();

      // If page provided doesn't exist we return bad request.
      if (page > ((totalMovies
                   / itemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      // We create the paged query request.
      var pagedMovies = await moviesQuery
        .Skip(toSkip)
        .Take(itemsPerPage)
        .ToListAsync();

      List<MovieDto> resultDtos = new();

      foreach (var pagedMovie in pagedMovies)
      {
        var actors = await Context.MovieActors.Include(x => x.Actor).Where(x => x.Movie == pagedMovie).ToListAsync();
        var actorsDto = actors.Select(x => Mapper.Map<Actor, ActorDto>(x.Actor)).ToList();

        var genres = await Context.MovieGenres.Include(x => x.Genre).Where(x => x.Movie == pagedMovie).ToListAsync();
        var genresDto = genres.Select(x => Mapper.Map<Genre, GenreDto>(x.Genre)).ToList();

        var directors = await Context.MovieDirectors.Include(x => x.Director).Where(x => x.Movie == pagedMovie).ToListAsync();
        var directorsDto = directors.Select(x => Mapper.Map<Director, DirectorDto>(x.Director)).ToList();

        var writers = await Context.MovieWriters.Include(x => x.Writer).Where(x => x.Movie == pagedMovie).ToListAsync();
        var writersDto = writers.Select(x => Mapper.Map<Writer, WriterDto>(x.Writer)).ToList();

        var countries = await Context.MovieCountries.Include(x => x.Country).Where(x => x.Movie == pagedMovie).ToListAsync();
        var countriesDto = countries.Select(x => Mapper.Map<Country, CountryDto>(x.Country)).ToList();

        var posters = await Context.Posters.Where(x => x.Movie == pagedMovie).Select(x => x.Url).ToListAsync();

        var screenshots = await Context.Screenshots.Where(x => x.Movie == pagedMovie).Select(x=>x.Url).ToListAsync();

        resultDtos.Add(new MovieDto
        {
          Actors = actorsDto,
          Directors = directorsDto,
          Writers = writersDto,
          Genres = genresDto,
          Countries = countriesDto,
          Posters = posters,
          Screenshots = screenshots,
          Featured = false,
          Title = pagedMovie.Title,
          TrailerUrl = pagedMovie.TrailerUrl,
          Duration = pagedMovie.Duration,
          ReleaseDate = pagedMovie.ReleaseDate,

        });
      }
      // We create the result, which is paged.
      var result = new PagedResult<MovieDto>
      {
        Results = resultDtos,
        Page = page,
        TotalPages = (totalMovies / itemsPerPage) + 1,
        TotalElements = totalMovies,
      };

      // We return OK and the paged results;
      return Ok(result);
    }

    [HttpPost("add")]
    public async Task<ActionResult> AddMovie([FromBody] AddMovieDto dto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState.Values.SelectMany(c => c.Errors));
      }

      var actorIds = dto.ActorIds.Distinct().OrderBy(x => x).ToList();
      var directorIds = dto.DirectorIds.Distinct().OrderBy(x => x).ToList();
      var writerIds = dto.WriterIds.Distinct().OrderBy(x => x).ToList();
      var genreIds = dto.GenreIds.Distinct().OrderBy(x => x).ToList();
      var posterUrls = dto.PosterUrls.Distinct().ToList();
      var screenshotUrls = dto.ScreenshotUrls.Distinct().ToList();
      var countryIds = dto.CountryIds.Distinct().OrderBy(x => x).ToList();

      var actors = await Context.Actors.Where(x => actorIds.Contains(x.Id)).ToListAsync();
      var genres = await Context.Genres.Where(x => genreIds.Contains(x.Id)).ToListAsync();
      var directors = await Context.Directors.Where(x => directorIds.Contains(x.Id)).ToListAsync();
      var writers = await Context.Writers.Where(x => writerIds.Contains(x.Id)).ToListAsync();
      var countries = await Context.Countries.Where(x => countryIds.Contains(x.Id)).ToListAsync();

      var missingDirectors = directorIds.Except(directors.Select(a => a.Id)).ToList();
      var missingActors = actorIds.Except(actors.Select(a => a.Id)).ToList();
      var missingGenres = genreIds.Except(genres.Select(a => a.Id)).ToList();
      var missingWriters = writerIds.Except(writers.Select(a => a.Id)).ToList();
      var missingCountries = countryIds.Except(countries.Select(a => a.Id)).ToList();

      if (missingCountries.Count != 0)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Country), missingCountries);
        return NotFound($"Could not find countries with ids {string.Join(", ", missingCountries)}");
      }

      if (missingActors.Count != 0)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Actor), missingActors);
        return NotFound($"Could not find actors with ids {string.Join(", ", missingActors)}");
      }

      if (missingDirectors.Count != 0)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Director), missingDirectors);
        return NotFound($"Could not find directors with ids {string.Join(", ", missingDirectors)}");
      }

      if (missingWriters.Count != 0)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Writer), missingWriters);
        return NotFound($"Could not find writers with ids {string.Join(", ", missingWriters)}");
      }

      if (missingGenres.Count != 0)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Genre), missingGenres);
        return NotFound($"Could not find genres with ids {string.Join(", ", missingGenres)}");
      }

      var movie = new Movie
      {
        Featured = false,
        Duration = TimeSpan.FromMinutes(dto.Duration),
        Plot = dto.Plot,
        TrailerUrl = dto.TrailerUrl,
        ReleaseDate = dto.ReleaseDate,
        Title = dto.Title,
      };

      var movieActors = actors.Select(x => new MovieActor { Actor = x, Movie = movie }).ToList();
      var movieDirectors = directors.Select(x => new MovieDirector { Director = x, Movie = movie }).ToList();
      var movieWriters = writers.Select(x => new MovieWriter { Writer = x, Movie = movie }).ToList();
      var movieGenres = genres.Select(x => new MovieGenre { Genre = x, Movie = movie }).ToList();
      var movieCountries = countries.Select(x => new MovieCountry { Country = x, Movie = movie }).ToList();
      var posters = posterUrls.Select(x => new Poster { Movie = movie, Url = x }).ToList();
      var screenshots = screenshotUrls.Select(x => new Screenshot { Movie = movie, Url = x }).ToList();

      Context.MovieActors.AddRange(movieActors);
      Context.MovieDirectors.AddRange(movieDirectors);
      Context.MovieWriters.AddRange(movieWriters);
      Context.MovieGenres.AddRange(movieGenres);
      Context.MovieCountries.AddRange(movieCountries);
      Context.Posters.AddRange(posters);
      Context.Screenshots.AddRange(screenshots);
      Context.Movies.Add(movie);

      await Context.SaveChangesAsync();

      return Ok();
    }

    /// <summary>
    /// This controller gets the list of featured movies.
    /// </summary>
    /// <param name="itemsPerPage">Define how many items shall be returned. </param>
    /// <param name="page">Choose which page of the results shall be returned.</param>
    /// <returns></returns>
    [HttpGet("featured")]
    public async Task<ActionResult<ICollection<MovieDto>>> GetAllFeaturedMovies(int itemsPerPage = 20, int page = 1)
    {
      var toSkip = itemsPerPage * (page - 1);

      var moviesQuery = Context.Movies
        .TagWith("Retrieving all featured movies")
        .Where(x => x.Featured.Equals(true))
        .OrderBy(x => x.Id);

      var totalMovies = await moviesQuery.CountAsync();

      if (page > ((totalMovies / itemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      var pagedMovies = await moviesQuery
        .Skip(toSkip)
        .Take(itemsPerPage)
        .ToListAsync();

      var result = new PagedResult<MovieDto>
      {
        Results = pagedMovies.Select(x => Mapper.Map<Movie, MovieDto>(x)).ToList(),
        Page = page,
        TotalPages = (totalMovies / itemsPerPage) + 1,
        TotalElements = totalMovies,
      };

      return Ok(result);
    }

    /// <summary>
    /// This controller sets the list of featured movies.
    /// </summary>
    /// <param name="featuredIdList"></param>
    /// <response code="200">All Kateuxein. </response>
    /// <response code="404">Couldn't match all id's to movies. </response>
    [HttpPost("featured")]
    public async Task<ActionResult<List<MovieDto>>> AddFeaturedMovie([FromBody] long[] featuredIdList, CancellationToken cancellationToken = default)
    {
      var movies = await Context.Movies.Where(m => featuredIdList.Contains(m.Id)).ToListAsync(cancellationToken);
      var temp = featuredIdList.Where(x => movies.All(y => y.Id != x)).ToArray();

      if (temp.Any())
      {
        return NotFound($"Couldn't find results for the given id's {string.Join(", ", temp)}");
      }

      foreach (var item in movies)
      {
        item.Featured = true;
      }

      await Context.SaveChangesAsync(cancellationToken);

      return Ok(movies.Select(x => Mapper.Map<Movie, MovieDto>(x)));
    }

    /// <summary>
    /// This controller sets the list of featured movies.
    /// </summary>
    /// <param name="featuredIdList"></param>
    /// <returns>No Content.</returns>
    [HttpPost("unfeatured")]
    public async Task<ActionResult> RemoveFeaturedMovie([FromBody] List<long> UnfeaturedIdList)
    {
      Context.Movies.Where(m => UnfeaturedIdList.Contains(m.Id)).ToList().ForEach(mv => mv.Featured = false);

      await Context.SaveChangesAsync();

      return NoContent();
    }
  }
}
