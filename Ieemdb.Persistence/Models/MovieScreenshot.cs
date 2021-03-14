using Ieemdb.Persistence.Base;
using Kritikos.Configuration.Persistence.Abstractions;
using System;

namespace Ieemdb.Persistence.Models
{
    public class MovieScreenshot : Entity<long>, IAuditable<Guid>
    {
        public Movie Movie { get; set; }
        public Image Screenshot { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
