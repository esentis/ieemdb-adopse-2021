namespace Esentis.Ieemdb.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.IdentityModel.Tokens.Jwt;
  using System.Linq;
  using System.Security.Claims;
  using System.Text;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Web;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Identity;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models;
  using Esentis.Ieemdb.Web.Models.Dto;
  using Esentis.Ieemdb.Web.Options;
  using Esentis.Ieemdb.Web.Views.Emails;

  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Identity.UI.Services;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;
  using Microsoft.IdentityModel.Tokens;

  using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

  [Authorize]
  [Route("api/account")]
  public class AccountController : BaseController<AccountController>
  {
    private readonly RoleManager<IeemdbRole> roleManager;
    private readonly UserManager<IeemdbUser> userManager;
    private readonly RazorViewToStringRenderer renderer;
    private readonly IEmailSender emailSender;
    private readonly JwtOptions jwtOptions;

    public AccountController(
      ILogger<AccountController> logger,
      IeemdbDbContext ctx,
      IPureMapper mapper,
      RoleManager<IeemdbRole> roleManager,
      UserManager<IeemdbUser> userManager,
      IOptions<JwtOptions> options,
      IEmailSender sender,
      RazorViewToStringRenderer renderer)
      : base(logger, ctx, mapper)
    {
      this.roleManager = roleManager;
      this.userManager = userManager;
      this.renderer = renderer;
      emailSender = sender;
      jwtOptions = options.Value;
    }

    /// <summary>
    /// Registers a User.
    /// </summary>
    /// <param name="userRegister">User information.</param>
    /// <response code="200">User successfully registered.</response>
    /// <response code="400">Validation Errors.</response>
    /// <response code="409">Registration errors.</response>
    /// <returns>No content.</returns>
    [AllowAnonymous]
    [HttpPost("")]
    public async Task<ActionResult> RegisterUser([FromBody] UserRegisterDto userRegister)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState.Values.SelectMany(c => c.Errors));
      }

      var user = new IeemdbUser { Email = userRegister.Email, UserName = userRegister.UserName, };

      var result = await userManager.CreateAsync(user, userRegister.Password);

      if (!result.Succeeded)
      {
        return Conflict(result.Errors);
      }

      var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
      var url =
        $"{Request.Scheme}://{Request.Host}{Request.PathBase}/api/account/confirm?email="
        + HttpUtility.UrlEncode($"{userRegister.Email}") + "&token=" + HttpUtility.UrlEncode($"{token}");
      var body = await renderer.RenderViewToStringAsync(
        "/Views/Emails/ConfirmAccountEmail.cshtml",
        new ConfirmAccountViewModel { ConfirmUrl = url, });
      await emailSender.SendEmailAsync(user.Email, "Email  Confirmation", body);

      await userManager.AddToRoleAsync(user, RoleNames.Member);
      return !result.Succeeded
        ? Conflict(result.Errors)
        : Ok();
    }

    /// <summary>
    /// Confirms a User.
    /// </summary>
    /// <param name="email">User's email.</param>
    /// <param name="token">Token generated.</param>
    /// <response code="200">User successfully registered.</response>
    /// <response code="409">Registration errors.</response>
    /// <returns>No content.</returns>
    [AllowAnonymous]
    [HttpGet("confirm")]
    public async Task<ActionResult> ConfirmEmail(string email, string token)
    {
      var user = await userManager.FindByEmailAsync(email);
      var result = await userManager.ConfirmEmailAsync(user, token);

      return !result.Succeeded
        ? Conflict(result.Errors)
        : this.Redirect($"{Request.Scheme}://{Request.Host}{Request.PathBase}");
    }

    /// <summary>
    /// Changes User's password.
    /// </summary>
    /// <param name="dto">Old and New password.</param>
    /// <response code="200">Password successfully changed.</response>
    /// <response code="404">User not found.</response>
    /// <response code="409">Password errors.</response>
    /// <returns>No content.</returns>
    [HttpPost("changePassword")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
      var userId = RetrieveUserId().ToString();
      var user = await userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return NotFound("User not found.");
      }

      var result = await userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
      if (!result.Succeeded)
      {
        return Conflict(result.Errors);
      }

      return Ok();
    }

    /// <summary>
    /// Changes User's username.
    /// </summary>
    /// <param name="username">New username.</param>
    /// <response code="200">Username successfully changed.</response>
    /// <response code="404">User not found.</response>
    /// <returns>No content.</returns>
    [HttpPost("changeUsername")]
    public async Task<ActionResult> ChangeUsername(string username, CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();
      var user = await userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return NotFound("User not found.");
      }

      user.UserName = username;
      user.NormalizedUserName = username.NormalizeSearch();

      await Context.SaveChangesAsync(token);
      return Ok();
    }

    /// <summary>
    /// Removes a User.
    /// </summary>
    /// <response code="204">User successfully removed.</response>
    /// <response code="404">User not found.</response>
    /// <returns>No content.</returns>
    [HttpDelete("")]
    public async Task<ActionResult> RemoveUser(CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();
      var user = await userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return NotFound("User not found.");
      }

      var devicesToDelete = await Context.Devices.Where(d => d.User == user).ToListAsync(token);
      var ratingsToDelete = await Context.Ratings.Where(r => r.User == user).ToListAsync(token);
      var watchlistsToDelete = await Context.Watchlists.Where(w => w.User == user).ToListAsync(token);
      var favoritesToDelete = await Context.Favorites.Where(f => f.User == user).ToListAsync(token);

      Context.Devices.RemoveRange(devicesToDelete);
      Context.Ratings.RemoveRange(ratingsToDelete);
      Context.Watchlists.RemoveRange(watchlistsToDelete);
      Context.Favorites.RemoveRange(favoritesToDelete);

      var result = await userManager.DeleteAsync(user);
      if (!result.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      await Context.SaveChangesAsync(token);
      return NoContent();
    }

    /// <summary>
    /// Authenticates User.
    /// </summary>
    /// <param name="userLogin">Login credentials.</param>
    /// <response code="200">User successfully removed.</response>
    /// <response code="404">User not found or wrong password.</response>
    /// <returns><see cref="UserBindingDto"/>.</returns>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserBindingDto>> LoginUser([FromBody] UserLoginDto userLogin,
      CancellationToken token = default)
    {
      var user = await userManager.FindByNameAsync(userLogin.UserName)
                 ?? await userManager.FindByEmailAsync(userLogin.UserName);
      if (user == null || !await userManager.CheckPasswordAsync(user, userLogin.Password))
      {
        return NotFound("User not found or wrong password");
      }

      var device = await Context.Devices.FirstOrDefaultAsync(e => e.Name == userLogin.DeviceName, token);
      if (device == null)
      {
        device = new Device { User = user, Name = userLogin.DeviceName };
        Context.Devices.Add(device);
      }

      var accessTokenExpiration = DateTimeOffset.UtcNow.AddMinutes(jwtOptions.DurationInMinutes);

      var claims = await GenerateClaims(user);
      var jwt = GenerateJwt(claims, accessTokenExpiration);

      var refreshToken = Guid.NewGuid();
      device.RefreshToken = refreshToken;

      await Context.SaveChangesAsync(token);

      var dto = new UserBindingDto(jwt, accessTokenExpiration, refreshToken);

      return Ok(dto);
    }

    /// <summary>
    /// Refreshes User's access token.
    /// </summary>
    /// <param name="dto">Refresh token credentials.</param>
    /// <response code="200">Returns refresh token.</response>
    /// <response code="404">Token is not valid. Device not found.</response>
    /// <returns><see cref="UserBindingDto"/>.</returns>
    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<UserBindingDto>> RefreshToken([FromBody] UserRefreshTokenDto dto,
      CancellationToken token = default)
    {
      var principal = GetPrincipalFromExpiredToken(dto.ExpiredToken);

      var valid = Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);

      if (!valid)
      {
        return NotFound();
      }

      var durationToCheck = DateTimeOffset.Now.AddDays(-jwtOptions.RefreshTokenDurationInDays);
      var device = await Context.Devices.Where(x => x.RefreshToken == dto.RefreshToken && x.User.Id == userId)
        .Where(x => x.UpdatedAt < durationToCheck)
        .SingleOrDefaultAsync(token);
      if (device
          == null)
      {
        return NotFound();
      }

      var user = await userManager.FindByIdAsync(userId.ToString());
      var accessTokenExpiration = DateTimeOffset.UtcNow.AddMinutes(jwtOptions.DurationInMinutes);

      var claims = await GenerateClaims(user);
      var jwt = GenerateJwt(claims, accessTokenExpiration);

      device.RefreshToken = Guid.NewGuid();
      await Context.SaveChangesAsync(token);

      var result = new UserBindingDto(jwt, accessTokenExpiration, Guid.Parse(jwt));

      return Ok(result);
    }

    // This method generates claims, claims are basically the information that will be appended to the Jwt.
    private async Task<List<Claim>> GenerateClaims(IeemdbUser user)
    {
      var identityClaims = await userManager.GetClaimsAsync(user);
      var identityRoles = (await userManager.GetRolesAsync(user)).Select(x => new Claim(ClaimTypes.Role, x));
      var claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(
          JwtRegisteredClaimNames.Iat,
          DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
          ClaimValueTypes.Integer64),
      };
      claims.AddRange(identityClaims);
      claims.AddRange(identityRoles);
      return claims.ToList();
    }

    // This method generates a new valid token.
    private string GenerateJwt(ICollection<Claim> claims, DateTimeOffset expiration)
    {
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
      var token = new JwtSecurityToken(
        issuer: jwtOptions.Issuer,
        audience: jwtOptions.Audience,
        notBefore: DateTimeOffset.UtcNow.UtcDateTime,
        expires: expiration.UtcDateTime,
        claims: claims,
        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // This method does all the steps required to issue a new valid token from an expired one.
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
      var tokenValidationParameters = new TokenValidationParameters
      {
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
        ValidateLifetime = false,
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
      return securityToken is not JwtSecurityToken jwtSecurityToken ||
             !jwtSecurityToken.Header.Alg.Equals(
               SecurityAlgorithms.HmacSha256,
               StringComparison.InvariantCultureIgnoreCase)
        ? throw new SecurityTokenException("Invalid token")
        : principal;
    }
  }
}
