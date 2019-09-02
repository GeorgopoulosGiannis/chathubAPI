using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.DTO
{
    public class ImageWithEntitiesDTO
    {
        public int Id { get; set; }

        public string Path { get; set; }

        public int LikesCount { get; set; }
    }
}
