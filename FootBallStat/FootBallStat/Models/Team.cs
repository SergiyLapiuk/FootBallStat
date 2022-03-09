using System;
using System.Collections.Generic;

namespace FootBallStat
{
    public partial class Team
    {
        public Team()
        {
            MatchTeam1s = new HashSet<Match>();
            MatchTeam2s = new HashSet<Match>();
            Players = new HashSet<Player>();
        }

        public int Id { get; set; }
        public int CityId { get; set; }
        public string Name { get; set; } = null!;

        public virtual City City { get; set; } = null!;
        public virtual ICollection<Match> MatchTeam1s { get; set; }
        public virtual ICollection<Match> MatchTeam2s { get; set; }
        public virtual ICollection<Player> Players { get; set; }
    }
}
