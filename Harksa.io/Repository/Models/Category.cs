using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models
{
    public class Category
    {
        public int Id { get; set; }

        [StringLength(64, ErrorMessage = "Catégorie trop longue")]
        public string Name { get; set; }

        public virtual ICollection<GameCategory> GameCategories { get; set; }

    }
}
