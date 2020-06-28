using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Models
{
    public class Post : Entity
    {
        public string ImageId { get; set; }
        public Image Image { get; set; }
        public string Text { get; set; }
        public IList<Comment> Comments { get;  }
    }
}
