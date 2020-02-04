using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Models     
{
    public class Image : Entity
    {

        public string Path { get; set; }
        public IList<Post> Posts { get; set; }

    }
}
