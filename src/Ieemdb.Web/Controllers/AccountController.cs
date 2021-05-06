namespace Esentis.Ieemdb.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.IdentityModel.Tokens.Jwt;
  using System.Linq;
  using System.Security.Claims;
  using System.Text;
  using System.Threading.Tasks;
  using System.Web;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Identity;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models;
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

  [Route("api/account")]
  public class AccountController : BaseController<AccountController>
  {
    private readonly RoleManager<IeemdbRole> roleManager;
    private readonly UserManager<IeemdbUser> userManager;
    private readonly RazorViewToStringRenderer renderer;
    private readonly IEmailSender emailSender;
    private readonly JwtOptions jwtOptions;

    // Constructor
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

    [HttpPost("")]
    [AllowAnonymous]
    public async Task<ActionResult> RegisterUser([FromBody] UserRegisterDto userRegister)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState.Values.SelectMany(c => c.Errors));
      }

      var user = new IeemdbUser { Email = userRegister.Email, UserName = userRegister.UserName, };
      var result = await userManager.CreateAsync(user, userRegister.Password);

      var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
      var url = HttpUtility.HtmlEncode(
        $"{Request.Scheme}://{Request.Host}{Request.PathBase}/api/account/confirm?email={userRegister.Email}&token={token}");
      var body = await renderer.RenderViewToStringAsync(
        "/Views/Emails/ConfirmAccountEmail.cshtml",
        new ConfirmAccountViewModel { ConfirmUrl = url, });
      await emailSender.SendEmailAsync(user.Email, "Email  Confirmation", body);

      await userManager.AddToRoleAsync(user, RoleNames.Member);
      return !result.Succeeded
        ? Conflict(result.Errors)
        : Ok();
    }

    [AllowAnonymous]
    [HttpPost("confirm")]
    public async Task<ActionResult> ConfirmEmail(string email, string token)
    {
      var user = await userManager.FindByEmailAsync(email);
      var result = await userManager.ConfirmEmailAsync(user, token);

      return !result.Succeeded
        ? Conflict(result.Errors)
        : Ok();
    }

    [HttpPost("emailToken")]
    public async Task<ActionResult> RequestEmailConfirm(string email)
    {
      var user = await userManager.FindByEmailAsync(email);
      var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
      var callbackUrl = Url.Page("/Identity/Account/ConfirmEmail", null, new { token }, protocol: Request.Scheme);
      return Ok(callbackUrl);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<UserBindingDto>> LoginUser([FromBody] UserLoginDto userLogin)
    {
      var user = await userManager.FindByNameAsync(userLogin.UserName)
                 ?? await userManager.FindByEmailAsync(userLogin.UserName);
      if (user == null || !await userManager.CheckPasswordAsync(user, userLogin.Password))
      {
        return NotFound("User not found or wrong password");
      }

      var device = await Context.Devices.FirstOrDefaultAsync(e => e.Name == userLogin.DeviceName);
      if (device == null)
      {
        device = new Device { User = user, Name = userLogin.DeviceName };
        Context.Devices.Add(device);
      }

      var accessTokenExpiration = DateTimeOffset.UtcNow.AddMinutes(jwtOptions.DurationInMinutes);

      var claims = await GenerateClaims(user);
      var token = GenerateJwt(claims, accessTokenExpiration);

      var refreshToken = Guid.NewGuid();
      device.RefreshToken = refreshToken;

      await Context.SaveChangesAsync();

      var dto = new UserBindingDto(token, accessTokenExpiration, refreshToken,userManager.GetRolesAsync(user).Result.FirstOrDefault());

      return Ok(dto);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<UserBindingDto>> RefreshToken([FromBody] UserRefreshTokenDto dto)
    {
      var principal = GetPrincipalFromExpiredToken(dto.ExpiredToken);

      var valid = Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);

      if (!valid)
      {
        return NotFound();
      }

      var device = await Context.Devices.Where(x => x.RefreshToken == dto.RefreshToken && x.User.Id == userId)
        .SingleOrDefaultAsync();
      if (device
          == null)
      {
        return NotFound();
      }

      var user = await userManager.FindByIdAsync(userId.ToString());
      var accessTokenExpiration = DateTimeOffset.UtcNow.AddMinutes(jwtOptions.DurationInMinutes);

      var claims = await GenerateClaims(user);
      var token = GenerateJwt(claims, accessTokenExpiration);

      device.RefreshToken = Guid.NewGuid();
      await Context.SaveChangesAsync();

      var result = new UserBindingDto(token, accessTokenExpiration, Guid.Parse(token), userManager.GetRolesAsync(user).Result.FirstOrDefault());

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
