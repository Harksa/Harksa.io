using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Models.Helpers
{
    public class ShortGamesWithCategoryModel
    {
        public string Category { get; set; }

        public IEnumerable<ShortGameModel> ShortGameModels { get; set; }
    }
}
