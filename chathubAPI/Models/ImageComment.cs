using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Models
{
    public class ImageComment
    {
        public string CommentId { get; set;}
        public Comment Comment { get; set; }
        public string ImageId { get; set; }
        public Image Image{ get; set; }
    }
}
