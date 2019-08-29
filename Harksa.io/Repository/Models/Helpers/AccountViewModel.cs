using System.Collections.Generic;

namespace Repository.Models.Helpers
{
    public class AccountViewModel
    {
        public int Id { get; set; }

        public string AccountName { get; set; }

        public IEnumerable<ShortGameModel> GameCards { get; set; }
    }
}
