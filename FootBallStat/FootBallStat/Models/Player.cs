using System;
using System.Collections.Generic;

namespace FootBallStat
{
    public partial class Player
    {
        public Player()
        {
            PlayersInMatches = new HashSet<PlayersInMatch>();
        }

        public int Id { get; set; }
        public int TeamId { get; set; }
        public int PositionId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Name { get; set; } = null!;
        public int Number { get; set; }

        public virtual Position Position { get; set; } = null!;
        public virtual Team Team { get; set; } = null!;
        public virtual ICollection<PlayersInMatch> PlayersInMatches { get; set; }
    }
}
