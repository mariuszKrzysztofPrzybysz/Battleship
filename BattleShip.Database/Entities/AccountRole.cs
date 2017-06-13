using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BattleShip.Database.Entities
{
    public class AccountRole
    {
        #region Persisted fields

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AccountRoleId { get; set; }

        public long AccountId { get; set; }

        public byte RoleId { get; set; }

        #endregion

        #region Navigation properties

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }

        #endregion
    }
}