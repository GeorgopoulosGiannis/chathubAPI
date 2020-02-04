using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.DTO
{
    public class ProfileDTO
    {
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }

        public string Alias { get; set; }
        public string[] ImageURLs { get; set; }
    }
}
