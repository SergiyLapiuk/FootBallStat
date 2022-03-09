using System;
using System.Collections.Generic;

namespace FootBallStat
{
    public partial class Match
    {
        public Match()
        {
            PlayersInMatches = new HashSet<PlayersInMatch>();
        }

        public int Id { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public int ChampionshipId { get; set; }
        public DateTime Date { get; set; }

        public virtual Championship Championship { get; set; } = null!;
        public virtual Team Team1 { get; set; } = null!;
        public virtual Team Team2 { get; set; } = null!;
        public virtual ICollection<PlayersInMatch> PlayersInMatches { get; set; }
    }
}
