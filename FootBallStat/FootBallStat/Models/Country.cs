using System;
using System.Collections.Generic;

namespace FootBallStat
{
    public partial class Country
    {
        public Country()
        {
            Championships = new HashSet<Championship>();
            Cities = new HashSet<City>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Championship> Championships { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}
