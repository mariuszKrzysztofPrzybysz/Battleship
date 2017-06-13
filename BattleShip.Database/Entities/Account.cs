using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BattleShip.Database.Entities
{
    public enum Gender
    {
        Unknown,
        Female,
        Male
    }

    public class Account
    {
        #region Persisted fields

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AccountId { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MinLength(5)]
        [MaxLength(50)]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public byte[] Photo { get; set; }

        [Required]
        public Gender Gender { get; set; }

        #endregion

        #region Navigation properties

        public virtual ICollection<AccountRole> AccountRoles { get; set; }

        public virtual ICollection<Battle> Battles { get; set; }

        #endregion
    }
}