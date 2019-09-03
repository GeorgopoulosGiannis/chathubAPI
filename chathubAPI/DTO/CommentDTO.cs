using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.DTO
{
    public class CommentDTO
    {
        public string Content { get; set; }

        public int CommentId { get; set; }

        public string CommentByEmail { get; set; }

        public string CommentByAlias { get; set; }

    }
}
