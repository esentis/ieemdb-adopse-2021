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

    /// <summary>
    /// Get a single movie provided the ID.
    /// </summary>
    /// <param name="id">Movie's unique ID. </param>
    /// <returns>Movie</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDto>> GetMovie(long id, CancellationToken token = default)
    {
      var movie = await Context.Movies
        .Include(x => x.MovieActors)
        .ThenInclude(x => x.Actor)
        .Include(x => x.MovieDirectors)
        .ThenInclude(x => x.Director)
        .Include(x => x.MovieWriters)
        .ThenInclude(x => x.Writer)
        .Include(x => x.MovieGenres)
        .ThenInclude(x => x.Genre)
        .Include(x => x.MovieCountries)
        .ThenInclude(x => x.Country)
        .SingleOrDefaultAsync(m => m.Id == id, token);

      if (movie == null)
      {
        return NotFound("Movie not found");
      }

      var movieDto = Mapper.Map<Movie, MovieDto>(movie, "complete");
      return Ok(movieDto);
    }

    /// <summary>
    /// Searches movies based on multiple criteria.
    /// </summary>
    /// <param name="criteria">Search criteria. </param>
    /// <response code="200">Returns search results. </response>
    /// <response code="400">Page doesn't exist. </response>
    /// <response code="402">Wrong operator </response>
    /// <returns>Search results.</returns>
    [HttpPost("search")]
    public async Task<ActionResult<ICollection<MovieDto>>> SearchForMovie(
      [FromBody] MovieSearchCriteria criteria,
      CancellationToken token = default)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState.Values);
      }

      var query = RetrieveUserId() == Guid.Empty
        ? Context.Movies
          .Where(x => true)
          .OrderBy(x => x.Id)
        : Context.Movies
          .WhereIf(
            criteria.MinDuration != null,
            x => x.Duration >= criteria.MinDuration)
          .WhereIf(
            criteria.MaxDuration != null,
            x => x.Duration <= criteria.MaxDuration)
          .WhereIf(
            criteria.FromYear != null,
            x => x.ReleaseDate >= criteria.FromYear)
          .WhereIf(
            criteria.ToYear != null,
            x => x.ReleaseDate <= criteria.ToYear)
          .WhereIf(
            criteria.Actor != null,
            x => Context.MovieActors.Any(a =>
              a.Actor.NormalizedLastName.Contains(criteria.Actor!.NormalizeSearch()) && x.Id == a.Movie.Id))
          .WhereIf(
            criteria.Director != null,
            x => Context.MovieDirectors.Any(a =>
              a.Director.NormalizedLastName.Contains(criteria.Director!.NormalizeSearch()) && x.Id == a.Movie.Id))
          .WhereIf(
            criteria.Writer != null,
            x => Context.MovieWriters.Any(a =>
              a.Writer.NormalizedLastName.Contains(criteria.Writer!.NormalizeSearch()) && x.Id == a.Movie.Id))
          .WhereIf(
            criteria.MinRating != null,
            x => Context.Ratings.Any(r => r.Rate >= criteria.MinRating && r.Movie.Id == x.Id))
          .WhereIf(
            criteria.MaxRating != null,
            x => x.Ratings.Average(y => y.Rate) <= criteria.MaxRating)
          .WhereIf(
            criteria.Genres.Length > 0,
            x => x.MovieGenres.Any(gn => criteria.Genres.Contains(gn.Genre.Id)))
          .OrderBy(x => x.Id);

      query = query
        .WhereIf(criteria.IsFeatured != null, x => x.Featured == criteria.IsFeatured)
        .WhereIf(
          criteria.TitleCriteria != null,
          x => x.NormalizedTitle.Contains(criteria.TitleCriteria!.NormalizeSearch()) || EF.Functions
            .ToTsVector("english", x.NormalizedSearch)
            .Matches(criteria.TitleCriteria!))
        .WhereIf(
          criteria.PlotCriteria != null,
          x => EF.Functions.ToTsVector("english", x.NormalizedSearch).Matches(criteria.PlotCriteria!))
        .OrderBy(x => x.Id);

      var matchingMovies = await query.CountAsync(token);

      var slice = await query.Slice(criteria.Page, criteria.ItemsPerPage)
        .Project<Movie, MovieDto>(Mapper, "complete")
        .ToListAsync(token);

      PagedResult<MovieDto> results = new()
      {
        Page = criteria.Page,
        Results = slice,
        TotalElements = matchingMovies,
        TotalPages = (matchingMovies / criteria.ItemsPerPage) + 1,
      };

      return Ok(results);
    }

    [HttpPost("")]
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
        PosterUrl = dto.PosterUrl,
        Featured = false,
        Duration = TimeSpan.FromMinutes(dto.DurationInMinutes),
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
      var screenshots = screenshotUrls.Select(x => new Screenshot { Movie = movie, Url = x }).ToList();

      Context.MovieActors.AddRange(movieActors);
      Context.MovieDirectors.AddRange(movieDirectors);
      Context.MovieWriters.AddRange(movieWriters);
      Context.MovieGenres.AddRange(movieGenres);
      Context.MovieCountries.AddRange(movieCountries);
      Context.Screenshots.AddRange(screenshots);
      Context.Movies.Add(movie);

      await Context.SaveChangesAsync();

      return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MovieDto>> UpdateMovie(long id, [FromBody] UpdateMovieDto movieDto)
    {
      var movie = await Context.Movies.SingleOrDefaultAsync(x => x.Id == id);
      if (movie == null)
      {
        return NotFound();
      }

      Mapper.Map(movieDto, movie);
      await Context.SaveChangesAsync();

      var dto = Mapper.Map<Movie, MovieDto>(movie);

      return Ok(dto);
    }

    /// <summary>
    /// This controller sets the list of featured movies.
    /// </summary>
    /// <param name="featuredIdList"></param>
    /// <response code="200">All Kateuxein. </response>
    /// <response code="404">Couldn't match all id's to movies. </response>
    [HttpPost("feature")]
    public async Task<ActionResult<List<MovieDto>>> AddFeaturedMovie(
      [FromBody] long[] featuredIdList,
      CancellationToken cancellationToken = default)
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
    /// <param name="featuredIdList">List of movie IDs to put on featured list.</param>
    /// <returns>No Content.</returns>
    [HttpPost("unfeature")]
    public async Task<ActionResult> RemoveFeaturedMovie([FromBody] List<long> UnfeaturedIdList)
    {
      Context.Movies.Where(m => UnfeaturedIdList.Contains(m.Id)).ToList().ForEach(mv => mv.Featured = false);

      await Context.SaveChangesAsync();

      return NoContent();
    }

    /// <summary>
    /// Returns all time top movies.
    /// </summary>
    /// <returns>List of top 100 movies.</returns>
    [HttpGet("top")]
    public async Task<ActionResult<List<MovieDto>>> GetTop()
    {
      var topRatedMovieIds = await Context.Ratings.OrderByDescending(x => x.Rate)
        .Take(100)
        .Select(x => x.Movie.Id)
        .ToListAsync();

      var topMovies = await Context.Movies.Include(x => x.MovieActors)
        .ThenInclude(x => x.Actor)
        .Include(x => x.MovieDirectors)
        .ThenInclude(x => x.Director)
        .Include(x => x.MovieWriters)
        .ThenInclude(x => x.Writer)
        .Include(x => x.MovieGenres)
        .ThenInclude(x => x.Genre)
        .Include(x => x.MovieCountries)
        .ThenInclude(x => x.Country)
        .Where(x => topRatedMovieIds.Contains(x.Id))
        .ToListAsync();

      var moviesDto = topMovies.Select(x => Mapper.Map<Movie, MovieDto>(x, "complete"));

      return Ok(moviesDto);
    }

    /// <summary>
    /// Returns movies released the current month.
    /// </summary>
    /// <param name="criteria">Page results criteria.</param>
    /// <returns>List of movies.</returns>
    [HttpGet("new")]
    public async Task<ActionResult<ICollection<MovieDto>>> GetNewReleases(
      PaginationCriteria criteria,
      CancellationToken token = default)
    {
      var monthAgo = DateTimeOffset.Now.AddDays(-30);
      var now = DateTimeOffset.Now;

      var newMoviesQuery = Context.Movies.Include(x => x.MovieActors)
        .ThenInclude(x => x.Actor)
        .Include(x => x.MovieDirectors)
        .ThenInclude(x => x.Director)
        .Include(x => x.MovieWriters)
        .ThenInclude(x => x.Writer)
        .Include(x => x.MovieGenres)
        .ThenInclude(x => x.Genre)
        .Include(x => x.MovieCountries)
        .ThenInclude(x => x.Country)
        .Where(x => (x.ReleaseDate <= now) && (x.ReleaseDate >= monthAgo))
        .OrderBy(x => x.ReleaseDate);

      var moviesCount = await newMoviesQuery.CountAsync(token);

      var slice = await newMoviesQuery.Slice(criteria.Page, criteria.ItemsPerPage)
        .Project<Movie, MovieDto>(Mapper, "complete")
        .ToListAsync(token);

      PagedResult<MovieDto> results = new()
      {
        Page = criteria.Page,
        Results = slice,
        TotalElements = moviesCount,
        TotalPages = (moviesCount / criteria.ItemsPerPage) + 1,
      };
      return Ok(results);
    }

    /// <summary>
    /// Returns movies released the current week.
    /// </summary>
    /// <param name="criteria">Page results criteria.</param>
    /// <returns>List of movies.</returns>
    [HttpGet("latest")]
    public async Task<ActionResult<ICollection<MovieDto>>> GetLatestAdded(
      PaginationCriteria criteria,
      CancellationToken token = default)
    {
      var weekAgo = DateTimeOffset.Now.AddDays(-7);
      var now = DateTimeOffset.Now;

      var latestMoviesQuery = Context.Movies.Include(x => x.MovieActors)
        .ThenInclude(x => x.Actor)
        .Include(x => x.MovieDirectors)
        .ThenInclude(x => x.Director)
        .Include(x => x.MovieWriters)
        .ThenInclude(x => x.Writer)
        .Include(x => x.MovieGenres)
        .ThenInclude(x => x.Genre)
        .Include(x => x.MovieCountries)
        .ThenInclude(x => x.Country)
        .Where(x => (x.CreatedAt <= now) && (x.CreatedAt >= weekAgo))
        .OrderBy(x => x.CreatedAt);

      var moviesCount = await latestMoviesQuery.CountAsync(token);

      var slice = await latestMoviesQuery.Slice(criteria.Page, criteria.ItemsPerPage)
        .Project<Movie, MovieDto>(Mapper, "complete")
        .ToListAsync(token);

      PagedResult<MovieDto> results = new()
      {
        Page = criteria.Page,
        Results = slice,
        TotalElements = moviesCount,
        TotalPages = (moviesCount / criteria.ItemsPerPage) + 1,
      };

      return Ok(results);
    }

    /// <summary>
    /// Deletes a movie from database.
    /// </summary>
    /// <param name="id">Movie's unique ID.</param>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMovie(long id)
    {
      var movie = await Context.Movies.SingleOrDefaultAsync(x => x.Id == id);

      if (movie == null)
      {
        return NotFound("Movie not found");
      }

      var movieRatings = await Context.Ratings.Where(x => x.Movie.Id == id).ToListAsync();
      var movieActors = await Context.MovieActors.Where(x => x.Movie.Id == id).ToListAsync();
      var movieDirectors = await Context.MovieDirectors.Where(x => x.Movie.Id == id).ToListAsync();
      var movieWriters = await Context.MovieWriters.Where(x => x.Movie.Id == id).ToListAsync();
      var movieGenres = await Context.MovieGenres.Where(x => x.Movie.Id == id).ToListAsync();
      var movieCountries = await Context.MovieCountries.Where(x => x.Movie.Id == id).ToListAsync();
      var movieWatchlist = await Context.MovieWatchlists.Where(x => x.Movie.Id == id).ToListAsync();
      var movieScreenshots = await Context.Screenshots.Where(x => x.Movie.Id == id).ToListAsync();

      Context.Ratings.RemoveRange(movieRatings);
      Context.Screenshots.RemoveRange(movieScreenshots);
      Context.MovieActors.RemoveRange(movieActors);
      Context.MovieDirectors.RemoveRange(movieDirectors);
      Context.MovieWriters.RemoveRange(movieWriters);
      Context.MovieGenres.RemoveRange(movieGenres);
      Context.MovieCountries.RemoveRange(movieCountries);
      Context.MovieWatchlists.RemoveRange(movieWatchlist);
      Context.Movies.Remove(movie);

      await Context.SaveChangesAsync();
      return NoContent();
    }
  }
}
