using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BattleShip.Database.Entities
{
    public class Role
    {
        #region Navigation properties

        public virtual ICollection<AccountRole> AccountRoles { get; set; }

        #endregion

        #region Persisted fields

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte RoleId { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        #endregion
    }
}