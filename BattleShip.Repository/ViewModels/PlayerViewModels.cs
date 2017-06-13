using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BattleShip.Database.Entities;

namespace BattleShip.Repository.ViewModels
{
    public class AddPlayerViewModel
    {
        [Required]
        [Index(IsUnique = true)]
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
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [Display(Name = "Allow new battle")]
        public bool AllowNewBattle { get; set; }

        [Required]
        [Display(Name = "Allow private chat")]
        public bool AllowPrivateChat { get; set; }
    }

    public class PlayerDetailsViewModel
    {
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Photo")]
        public byte[] Photo { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Allow new battle")]
        public string AllowNewBattle { get; set; }

        [Display(Name = "Allow private chat")]
        public string AllowPrivateChat { get; set; }
    }
}