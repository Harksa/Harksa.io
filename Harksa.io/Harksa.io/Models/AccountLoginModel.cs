using System.ComponentModel.DataAnnotations;

namespace Harksa.io.Models
{
    public class AccountLoginModel
    {
        [Required]
        [StringLength(maximumLength:64, ErrorMessage = "Nom de compte trop grand")]
        [Display(Name = "Pseudo")]
        public string AccountName { get; set; }

        [Required]
        [StringLength(maximumLength:64, ErrorMessage = "Mot de passe trop grand")]
        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Display(Name = "Se souvenir de moi")]
        public bool RememberMe { get; set; }
    }
}
