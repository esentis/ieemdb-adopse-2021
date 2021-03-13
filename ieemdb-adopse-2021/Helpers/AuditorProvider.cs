using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Kritikos.Configuration.Persistence.Services;
using Microsoft.AspNetCore.Http;

namespace ieemdb_adopse_2021.Helpers
{
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
