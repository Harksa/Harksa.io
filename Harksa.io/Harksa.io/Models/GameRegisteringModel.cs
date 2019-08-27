using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Harksa.io.Validators;
using Microsoft.AspNetCore.Http;

namespace Harksa.io.Models
{
    public class GameRegisteringModel
    {
        [Required(ErrorMessage = "Un titre est requis")]
        [StringLength(100, ErrorMessage = "Titre trop grand")]
        [Display(Name = "Titre")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description courte requise")]
        [StringLength(64, ErrorMessage = "Description courte trop grande")]
        [Display(Name = "Description courte")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Description requise")]
        [StringLength(maximumLength:1024, ErrorMessage = "Description trop grande")]
        [Display(Name = "Description longue")]
        public string Description { get; set; }

        [StringLength(32, ErrorMessage = "Nombre de caractères dépassés")]
        [Display(Name = "Tags")]
        [ValidNumbersOfCategories(ErrorMessage = "Limite de 3 tags dépassée")]
        public string Categories { get; set; }

        [Required(ErrorMessage = "Une image du jeu est requise")]
        [Display(Name = "Image")]
        [ValidFileFormat(AcceptedFormats = ".jpg,.gif,.png", ErrorMessage = "Format de fichier non accepté")]
        public IFormFile GamePicture { get; set; }
    }
}
