using System.ComponentModel.DataAnnotations;

namespace Harksa.io.Models
{
    public class AccountRegisterModel
    {
        [Required]
        [Display(Name = "Pseudo")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmez le mot de passe")]
        [Compare("Password", ErrorMessage = "Mots de passes non identique")]
        public string ConfirmPassword { get; set; }
    }
}
