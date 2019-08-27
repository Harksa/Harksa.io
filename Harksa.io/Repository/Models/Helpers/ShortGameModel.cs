using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models.Helpers
{
    public class ShortGameModel
    {
        public int Id { get; set; }

        public int AccountId {get ; set; }

        public string Title { get; set; }

        public string ImageFileName { get; set; }

        public string ShortDescription { get; set; }

        [Display(Name = "Tags")]
        public IEnumerable<string> Categories { get; set; }
    }
}
