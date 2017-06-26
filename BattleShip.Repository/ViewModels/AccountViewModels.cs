using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BattleShip.Database.Entities;

namespace BattleShip.Repository.ViewModels
{
    public class AddAccountViewModel
    {
        [Required]
        [Index(IsUnique = true)]
        [MinLength(5)]
        [MaxLength(50)]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }

        [MaxLength(50)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }
    }

    public class EditAccountViewModel
    {
        [Required]
        public long AccountId { get; set; }

        [MaxLength(50)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Photo")]
        public byte[] Photo { get; set; }

        [Required]
        [Display(Name = "Allow new battle")]
        public bool AllowNewBattle { get; set; }

        [Required]
        [Display(Name = "Allow private chat")]
        public bool AllowPrivateChat { get; set; }
    }

    public class AccountPermissionsViewModel
    {
        public string Login { get; set; }

        public bool AllowNewBattle { get; set; }

        public bool AllowPrivateChat { get; set; }
    }
}