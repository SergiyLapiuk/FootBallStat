using System;
using System.Collections.Generic;

namespace FootBallStat
{
    public partial class PlayersInMatch
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int MatchId { get; set; }
        public int PlayerGoals { get; set; }

        public virtual Match Match { get; set; } = null!;
        public virtual Player Player { get; set; } = null!;
    }
}
