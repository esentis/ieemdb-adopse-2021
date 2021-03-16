using Ieemdb.Persistence.Base;
using Ieemdb.Persistence.Identity;
using Kritikos.Configuration.Persistence.Abstractions;
using System;

namespace Ieemdb.Persistence.Models
{
    public class Favorite : Entity<long>, IAuditable<Guid>
    {
        public IeemdbUser User { get; set; }
        public Movie Movie { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
