using System;
using System.Collections.Generic;

namespace FootBallStat
{
    public partial class City
    {
        public City()
        {
            Teams = new HashSet<Team>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; } = null!;

        public virtual Country Country { get; set; } = null!;
        public virtual ICollection<Team> Teams { get; set; }
    }
}
