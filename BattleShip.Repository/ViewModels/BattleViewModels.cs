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

    public class PlayerBattlesViewModel
    {
        [Display(Name = "Id")]
        public long BattleId { get; set; }

        public string Player { get; set; }

        public string Opponent { get; set; }

        [Display(Name="UTC start date and time")]
        public DateTime StartUtcDateTime { get; set; }

        [Display(Name = "Duration (in seconds)")]
        public int? Duration { get; set; }

        public string Winner { get; set; }
    }
}