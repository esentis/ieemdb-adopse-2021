namespace Esentis.Ieemdb.Web.Providers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Web.Models.MovieDBDto;
  using Esentis.Ieemdb.Web.Providers.Models;

  using Refit;

  [Headers("Authorization: Bearer")]
  public interface ITheMovieDb
  {
    [Get("/search/person")]
    Task<SearchResult<PersonSearch>> SearchPeople(string query, int page = 1);

    [Get("/person/{id}")]
    Task<PersonDB> GetPerson(long id);

    [Get("/genre/movie/list")]
    Task<GenreResults> GetGenres();

    [Get("/movie/popular")]
    Task<SearchResult<MovieDB>> GetRecommended();

    [Get("/movie/{id}")]
    Task<DetailedMovie> GetMovieDetails(long id);

    [Get("/movie/{id}/credits")]
    Task<MovieCast> GetMovieCredits(long id);

  }
}
