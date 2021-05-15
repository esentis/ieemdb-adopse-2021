namespace Esentis.Ieemdb.Web.Models
{
  using System;
  using System.ComponentModel;
  using System.ComponentModel.DataAnnotations;

  using Esentis.Ieemdb.Web.Models.Enums;

  public record AddPersonDto(string FullName, DateTime BirthDate, DateTime DeathDate, string Bio, string Image);

  public record AddGenreDto(string Name);

  public record UserRegisterDto(
    [Required] string UserName,
    [Required][EmailAddress] string Email,
    [Required][PasswordPropertyText] string Password);

  public record UserLoginDto(
    [Required] string UserName,
    [Required][PasswordPropertyText] string Password,
    [Required] string DeviceName);

  public record UserBindingDto(string AccessToken, DateTimeOffset ExpiresIn, Guid RefreshToken);

  public record UserRefreshTokenDto(string ExpiredToken, string Device, Guid RefreshToken);
}
