using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Models
{
    public class FcmToken
    {
        public string Token { get; set; }
        public string TokenOwner { get; set; }
        public User User { get; set; }
    }
}
