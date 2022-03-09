using System;
using System.Collections.Generic;

namespace FootBallStat
{
    public partial class Position
    {
        public Position()
        {
            Players = new HashSet<Player>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Player> Players { get; set; }
    }
}
