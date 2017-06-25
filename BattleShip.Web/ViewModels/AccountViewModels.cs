using System.ComponentModel.DataAnnotations;
using System.Web;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Web.ViewModels
{
    public class SignUpAccountViewModel : AddAccountViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class SignInAccountViewModel
    {
        [Required]
        [MaxLength(50)]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class ExtendedEditAccountViewModel : EditAccountViewModel
    {
        [DataType(DataType.Upload)]
        [Display(Name = "Photo")]
        public HttpPostedFileBase AccountPhoto { get; set; }
    }
}