using System;

namespace Repository.Models.Helpers
{
    public class FullGameModel : ShortGameModel
    {
        public string Author { get; set; }

        public string FullDescription { get; set; }

        public DateTime PublicationTime { get; set; }
    }
}
