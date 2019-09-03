using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Models.Helpers
{
    public class CommentClientSideModel
    {
        public string AuthorName { get; set; }

        public int Score { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
