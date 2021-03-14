using Ieemdb.Persistence.Base;
using Kritikos.Configuration.Persistence.Abstractions;
using System;

namespace Ieemdb.Persistence.Models
{
    public class Image : Entity<long>, IAuditable<Guid>
    {
        public string Url { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
