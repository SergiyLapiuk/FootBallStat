using System;
using System.Collections.Generic;

namespace FootBallStat
{
    public partial class Country
    {
        public Country()
        {
            Championships = new HashSet<Championship>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Championship> Championships { get; set; }
    }
}
