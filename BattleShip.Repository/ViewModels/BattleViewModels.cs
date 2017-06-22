using System;
using System.ComponentModel.DataAnnotations;

namespace BattleShip.Repository.ViewModels
{
    public class CreateBattleViewModel
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "Player name")]
        public string PlayerName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Opponent name")]
        public string OpponentName { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartUtcDateTime { get; set; }
    }

    public class PlayBattleViewModel
    {
        public string PlayerBoard { get; set; }

        public string Opponent { get; set; }
    }
}
