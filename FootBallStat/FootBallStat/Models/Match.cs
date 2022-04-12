using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootBallStat
{
    public partial class Match
    {
        public Match()
        {
            PlayersInMatches = new HashSet<PlayersInMatch>();
        }

        public int Id { get; set; }
        [Display(Name = "Команда-господар")]
        public int Team1Id { get; set; }
        [Display(Name = "Команда-гість")]
        public int Team2Id { get; set; }
        [Display(Name = "Чемпіонат")]
        public int ChampionshipId { get; set; }
        [Display(Name = "Дата та час")]
        public DateTime Date { get; set; }

        [Display(Name = "Чемпіонат")]
        public virtual Championship Championship { get; set; } = null!;
        [Display(Name = "Команда-господар")]
        public virtual Team Team1 { get; set; } = null!;
        [Display(Name = "Команда-гість")]
        public virtual Team Team2 { get; set; } = null!;
        public virtual ICollection<PlayersInMatch> PlayersInMatches { get; set; }
    }
}
