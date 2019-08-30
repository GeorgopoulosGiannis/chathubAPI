using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Models
{
    public class LikedImage
    {
        public int ImageId { get; set; }
        public Image Image { get; set; }
        public string LikedById { get; set; }
        public User User { get; set; }
    }
}
