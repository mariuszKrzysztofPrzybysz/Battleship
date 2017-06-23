using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BattleShip.Database.Entities
{
    public class Battle
    {
        #region Persisted fields

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BattleId { get; set; }

        [Required]
        public long PlayerId { get; set; }

        [Required]
        public long OpponentId { get; set; }

        [MaxLength(500)]
        public string PlayerBoard { get; set; }

        [MaxLength(500)]
        public string OpponentBoard { get; set; }

        public bool PlayerIsReady { get; set; }

        public bool OpponentIsReady { get; set; }

        public long Attacker { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartUtcDateTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? EndUtcDateTime { get; set; }

        public long? WinnerId { get; set; }

        #endregion

        #region Navigation properties

        [ForeignKey(nameof(PlayerId))]
        public virtual Account Player { get; set; }

        [ForeignKey(nameof(OpponentId))]
        public virtual Account Opponent { get; set; }

        #endregion
    }
}