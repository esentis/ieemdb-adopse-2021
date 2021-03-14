using System;
using Ieemdb.Persistence.Base;
using Kritikos.Configuration.Persistence.Abstractions;

namespace Ieemdb.Persistence.Models
{
    public class MovieDirector : Entity<long>, IAuditable<Guid>
    {
        public Movie Movie { get; set; }
        public Director Director { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
