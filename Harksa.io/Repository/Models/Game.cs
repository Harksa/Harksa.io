using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models
{
    public class Game
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public virtual Account Account { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Titre trop grand")]
        public string Title { get; set; }

        [Required]
        [StringLength(maximumLength:64, ErrorMessage = "Description trop grande")]
        public string ShortDescription { get; set; }

        [StringLength(maximumLength:1024, ErrorMessage = "Description trop grande")]
        public string Description { get; set; }

        [Required]
        public string ImageFileName { get; set; }

        public virtual ICollection<GameCategory> GameCategories { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
