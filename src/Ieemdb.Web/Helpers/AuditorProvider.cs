namespace Esentis.Ieemdb.Web.Helpers
{
  using System;
  using System.Security.Claims;

  using Kritikos.Configuration.Persistence.Services;

  using Microsoft.AspNetCore.Http;

  public class AuditorProvider : IAuditorProvider<Guid>
  {
    private readonly IHttpContextAccessor accessor;

    public AuditorProvider(IHttpContextAccessor accessor) => this.accessor = accessor;

    public Guid GetAuditor() => Guid.TryParse(
        accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
        out var guid)
        ? guid
        : GetFallbackAuditor();

    public Guid GetFallbackAuditor() => Guid.Empty;
  }
}
