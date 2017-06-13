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

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? StartUtcDateTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? EndUtcDateTime { get; set; }

        public long? WinnerId { get; set; }

        #endregion
    }
}