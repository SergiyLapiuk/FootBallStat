using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootBallStat
{
    public partial class Country
    {
        public Country()
        {
            Championships = new HashSet<Championship>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage ="Поле не повинно бути порожнім")]
        [Display(Name = "Країна")]
        public string Name { get; set; } = null!;

        public virtual ICollection<Championship> Championships { get; set; }
    }
}
