using System;
using System.Collections.Generic;

namespace FootBallStat
{
    public partial class Championship
    {
        public Championship()
        {
            Matches = new HashSet<Match>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; } = null!;

        public virtual Country Country { get; set; } = null!;
        public virtual ICollection<Match> Matches { get; set; }
    }
}
