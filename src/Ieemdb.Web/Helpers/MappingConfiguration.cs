namespace Esentis.Ieemdb.Web.Helpers
{
  using System;
  using System.Linq;

  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Models;
  using Esentis.Ieemdb.Web.Models.Dto;

  using Kritikos.PureMap;
  using Kritikos.PureMap.Contracts;

  using Nessos.Expressions.Splicer;

  public static class MappingConfiguration
  {
    public static readonly IPureMapperConfig Mapping = new PureMapperConfig()
      .Map<Genre, GenreDto>(mapper => genre => new GenreDto() { Name = genre.Name, Id = genre.Id, })
      .Map<Country, CountryDto>(mapper =>
        country => new CountryDto() { Name = country.Name, Id = country.Id, Iso = country.Iso })
      .Map<GenreDto, Genre>(mapper => genreDto => new Genre() { Name = genreDto.Name, Id = genreDto.Id, })
      .Map<AddGenreDto, Genre>(mapper => addGenre => new Genre() { Name = addGenre.Name, })
      .Map<Movie, MovieMinimalDto>(mapper => movie => new MovieMinimalDto()
      {
        Id = movie.Id,
        Title = movie.Title,
        Plot = movie.Plot,
        Duration = movie.Duration,
        Featured = movie.Featured,
        ReleaseDate = movie.ReleaseDate,
        AverageRating = movie.AverageRating,
        PosterUrl = movie.PosterUrl,
        Genres = movie.MovieGenres.Select(x => mapper.Resolve<Genre, GenreDto>().Invoke(x.Genre)).ToList(),
      })
      .Map<Movie, MovieDto>(
        mapper => movie => new MovieDto()
        {
          Id = movie.Id,
          Title = movie.Title,
          Plot = movie.Plot,
          AverageRating = movie.AverageRating,
          PosterUrl = movie.PosterUrl,
          Duration = movie.Duration,
          Featured = movie.Featured,
          ReleaseDate = movie.ReleaseDate,
          People = movie.People.Select(x => mapper.Resolve<Person, PersonDto>().Invoke(x.Person)).ToList(),
          Genres = movie.MovieGenres.Select(x => mapper.Resolve<Genre, GenreDto>().Invoke(x.Genre)).ToList(),
          Countries =
            movie.MovieCountries.Select(x => mapper.Resolve<Country, CountryDto>().Invoke(x.Country)).ToList(),
        },
        name: "complete")
      .Map<UpdateMovieDto, Movie>(mapper => (source, destination) => UpdateMovie(source, destination, mapper))
      .Map<Rating, RatingDto>(mapper => rating => new RatingDto()
      {
        Id = rating.Id,
        MovieId = rating.Movie.Id,
        CreatedAt = rating.CreatedAt,
        MovieName = rating.Movie.Title,
        Rate = rating.Rate,
        Review = rating.Review,
      })
      .Map<Favorite, FavoriteDto>(mapper => favorite => new FavoriteDto()
      {
        Id = favorite.Id, Movie = mapper.Resolve<Movie, MovieDto>("complete").Invoke(favorite.Movie),
      })
      .Map<Person, PersonDto>(mapper => person => new PersonDto()
      {
        Id = person.Id,
        Bio = person.Bio,
        BirthDay = person.BirthDay,
        DeathDay = person.DeathDay,
        FullName = person.FullName,
        Image = person.Image,
        KnownFor = person.KnownFor.ToString(),
      })
      .Map<AddPersonDto, Person>(mapper => dto => new Person()
      {
        Bio = dto.Bio,
        BirthDay = dto.BirthDate,
        DeathDay = dto.DeathDate,
        FullName = dto.FullName,
        Image = dto.Image,
      })
      .Map<Image, ImageDto>(mapper => dto => new ImageDto() { Url = new Uri(dto.Url), Id = dto.Id, })
      .Map<Video, VideoDto>(mapper => dto => new VideoDto()
      {
        Id = dto.Id,
        TmdbId = dto.TmdbId,
        Key = dto.Key,
        Site = dto.Site,
        Type = dto.Type.ToString(),
      })
      .Map<Watchlist, WatchlistDto>(mapper => watchlist => new WatchlistDto()
      {
        Movie = mapper.Resolve<Movie, MovieDto>("complete").Invoke(watchlist.Movie), Id = watchlist.Id
      });

    private static Movie UpdateMovie(UpdateMovieDto dto, Movie movie, IPureMapperUpdateResolver mapper)
    {
      movie.ReleaseDate = dto.ReleaseDate;
      movie.Title = dto.Title;
      movie.Plot = dto.Plot;
      movie.Duration = dto.Duration;

      return movie;
    }
  }
}
