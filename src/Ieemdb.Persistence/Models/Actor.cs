namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Base;
  using Esentis.Ieemdb.Persistence.Helpers;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class Actor : Entity<long>, IAuditable<Guid>
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
