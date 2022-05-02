﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootBallStat
{
    public class CurrentDate1Attribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateTime = Convert.ToDateTime(value);
            var dateString = "1/1/2018 0:00:00 AM";
            DateTime date1 = DateTime.Parse(dateString,
                                      System.Globalization.CultureInfo.InvariantCulture);
            var dateString2 = "1/1/2024 0:00:00 AM";
            DateTime date2 = DateTime.Parse(dateString2,
                                      System.Globalization.CultureInfo.InvariantCulture);
            return date1 <= dateTime && dateTime <= date2;
        }
    }
    public partial class Match
    {
        public Match()
        {
            PlayersInMatches = new HashSet<PlayersInMatch>();
        }

        public int Id { get; set; }
        [Display(Name = "Команда-господар")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        public int Team1Id { get; set; }
        [Display(Name = "Команда-гість")]
        public int Team2Id { get; set; }
        [Display(Name = "Чемпіонат")]
        public int ChampionshipId { get; set; }
        [Display(Name = "Дата та час")]
        [CurrentDate1(ErrorMessage = "Дата матчу повинна бути від 01.01.2018 до 01.01.2024")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
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
