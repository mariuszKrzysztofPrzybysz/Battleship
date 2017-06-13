using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BattleShip.Database.Entities
{
    public class AccountRole
    {
        #region Persisted fields

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long PlayerId { get; set; }

        public byte RoleId { get; set; }

        #endregion
    }
}