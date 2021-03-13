using Kritikos.Configuration.Persistence.Abstractions;
using Microsoft.AspNetCore.Identity;
using System;

namespace Ieemdb.Persistence.Identity
{
    public class IeemdbRole : IdentityRole<Guid>, IEntity<Guid>, ITimestamped
    {
        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }
}
