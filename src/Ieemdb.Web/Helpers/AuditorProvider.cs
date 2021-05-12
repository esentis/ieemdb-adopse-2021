namespace Esentis.Ieemdb.Web.Helpers
{
  using System;
  using System.Security.Claims;

  using Kritikos.Configuration.Persistence.Interceptors.Services;

  using Microsoft.AspNetCore.Http;

  public class AuditorProvider : IAuditorProvider<Guid>
  {
    private static readonly Guid AppGuid = Guid.Parse("3b4be676-0354-5f47-b91f-e3416dc638fa");
    private readonly IHttpContextAccessor accessor;

    public AuditorProvider(IHttpContextAccessor accessor) => this.accessor = accessor;

    public Guid GetAuditor() => Guid.TryParse(
      accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
      out var guid)
      ? guid
      : GetFallbackAuditor();

    public Guid GetFallbackAuditor() => AppGuid;
  }
}
