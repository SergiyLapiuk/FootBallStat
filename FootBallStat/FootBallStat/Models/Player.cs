using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootBallStat
{
    public partial class Player
    {
        public Player()
        {
            PlayersInMatches = new HashSet<PlayersInMatch>();
        }

        public int Id { get; set; }
        [Display(Name = "Команда")]
        public int TeamId { get; set; }
        [Display(Name = "Позиція на полі")]
        public int PositionId { get; set; }
        [Display(Name = "Дата народження")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Ім'я")]
        public string Name { get; set; } = null!;
        [Display(Name = "Ігровий номер")]
        public int Number { get; set; }

        [Display(Name = "Позиція на полі")]
        public virtual Position Position { get; set; } = null!;
        [Display(Name = "Команда")]
        public virtual Team Team { get; set; } = null!;
        public virtual ICollection<PlayersInMatch> PlayersInMatches { get; set; }
    }
}
