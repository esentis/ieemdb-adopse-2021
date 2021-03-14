using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ieemdb.Persistence.Base;
using Kritikos.Configuration.Persistence.Abstractions;

namespace Ieemdb.Persistence.Models
{
    public class MovieWriter : Entity<long>, IAuditable<Guid>
    {
        public Writer Writer { get; set; }
        public Movie Movie { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
