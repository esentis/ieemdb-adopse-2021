namespace Esentis.Ieemdb.Web.Helpers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Linq.Expressions;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Models;
  using Esentis.Ieemdb.Web.Models.Dto;

  using Kritikos.PureMap;
  using Kritikos.PureMap.Contracts;

  using Nessos.Expressions.Splicer;

  public static class MappingConfiguration
  {
    public static readonly IPureMapperConfig Mapping = new PureMapperConfig()
      .Map<Actor, ActorDto>(mapper => actor => new ActorDto() { Name = actor.Name, Bio = actor.Bio, Id = actor.Id, BirthDate = actor.BirthDate, })
      .Map<ActorDto, Actor>(mapper => actorDto => new Actor() { Name = actorDto.Name, Bio = actorDto.Bio, Id = actorDto.Id, BirthDate = actorDto.BirthDate, })
      .Map<AddActorDto, Actor>(mapper => addActor => new Actor() { Name = addActor.Name, Bio = addActor.bio, BirthDate = addActor.birthDate, })
      .Map<Director, DirectorDto>(mapper => director => new DirectorDto() { Name = director.Name, Bio = director.Bio, Id = director.Id, BirthDate = director.BirthDate, })
      .Map<DirectorDto, Director>(mapper => directorDto => new Director() { Name = directorDto.Name, Bio = directorDto.Bio, Id = directorDto.Id, BirthDate = directorDto.BirthDate, })
      .Map<AddDirectorDto, Director>(mapper => addDirector => new Director() { Name = addDirector.Name, Bio = addDirector.bio, BirthDate = addDirector.birthDate, })
      .Map<WeatherForecast, MovieDto>(
        mapper => forecast => new MovieDto
        {
          Title = forecast.Summary,
          Actor = mapper.Resolve<WeatherForecast, ActorDto>().Invoke(forecast),
        },
        name: "withActor")
      .Map<WeatherForecast, ActorDto>(mapper =>
        forecast => new ActorDto() { Name = forecast.TemperatureC.ToString(), })
      .Map<MovieDto, Movie>(mapper => (dest, source) => UpdateMovie(source, dest))
      .Map<MovieDto, Movie>(mapper => (dest, source) => UpdateMovie(source, dest), name: "v2");

    private static Movie UpdateMovie(Movie movie, MovieDto dto)
    {
      return movie;
    }
  }
}
