namespace Esentis.Ieemdb.Web.Models
{
  using System;

  public record AddActorDto(string Name, DateTimeOffset birthDate, string bio);
  public record AddDirectorDto(string Name, DateTimeOffset birthDate, string bio);
  public record AddGenreDto(string Name);
}
