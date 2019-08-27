using System.Collections.Generic;

namespace Repository.Models
{
    public class Account
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public virtual ICollection<Game> Games { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
