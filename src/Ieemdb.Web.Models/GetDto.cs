namespace Esentis.Ieemdb.Web.Models
{
  using System;
  using System.ComponentModel;
  using System.ComponentModel.DataAnnotations;

  public record AddActorDto(string FirstName, string LastName, DateTimeOffset BirthDate, string Bio);

  public record AddDirectorDto(string FirstName, string LastName, DateTimeOffset BirthDate, string Bio);

  public record AddGenreDto(string Name);

  public record AddWriterDto(string FirstName, string LastName, DateTimeOffset BirthDate, string Bio);

  public record UserRegisterDto(
    [Required] string UserName,
    [Required][EmailAddress] string Email,
    [Required][PasswordPropertyText] string Password);

  public record UserLoginDto(
    [Required] string UserName,
    [Required][PasswordPropertyText] string Password,
    [Required] string DeviceName);

  public record UserBindingDto(string AccessToken, DateTimeOffset ExpiresIn, Guid RefreshToken, string role);

  public record UserRefreshTokenDto(string ExpiredToken, Guid RefreshToken);
}
