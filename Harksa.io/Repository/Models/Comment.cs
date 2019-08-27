using System.ComponentModel.DataAnnotations;

namespace Repository.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public virtual Account Account { get; set; }

        public int GameId { get; set; }

        public virtual Game Game { get; set; }

        [Range(1,5)]
        public int Score { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Titre trop grand")]
        [Display(Name = "Titre")]
        public string Title { get; set; }

        [StringLength(512, ErrorMessage = "Commentaire trop grand")]
        public string Content { get; set; }
    }
}
