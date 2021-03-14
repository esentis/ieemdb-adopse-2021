using System;
using Ieemdb.Persistence.Base;
using Kritikos.Configuration.Persistence.Abstractions;

namespace Ieemdb.Persistence.Models
{
    public class Genre : Entity<long>, IAuditable<Guid>
    {
        public string Name { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
