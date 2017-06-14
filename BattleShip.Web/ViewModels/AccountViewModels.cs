using System.ComponentModel.DataAnnotations;
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
}