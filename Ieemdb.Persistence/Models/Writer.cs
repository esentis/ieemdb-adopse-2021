using Ieemdb.Persistence.Helpers;
using System;
using Ieemdb.Persistence.Base;
using Kritikos.Configuration.Persistence.Abstractions;

namespace Ieemdb.Persistence.Models
{
    public class Writer : Entity<long>, IAuditable<Guid>
    {
        private string name = string.Empty;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                NormalizedName = value.NormalizeSearch();
            }
        }

        public string NormalizedName { get; private set; }

        public DateTimeOffset BirthDate { get; set; }

        public string Bio { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
