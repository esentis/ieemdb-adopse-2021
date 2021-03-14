using System;
using Ieemdb.Persistence.Base;
using Kritikos.Configuration.Persistence.Abstractions;

namespace Ieemdb.Persistence.Models
{
    public class MovieActor : Entity<long>, IAuditable<Guid>
    {
        public Movie Movie { get; set; }
        public Actor Actor { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
