using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.DTO
{
    public class UpdateCommentDTO
    {
        public int CommentId { get; set; }

        public string Content { get; set; }
    }
}
