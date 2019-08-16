using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Models
{
    public class ChatMessage
    {
        public string from { get; set; }

        public string message { get; set; }

        public string to { get; set; }
    }
}
