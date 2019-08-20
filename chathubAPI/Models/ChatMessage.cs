using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string from { get; set; }

        public string message { get; set; }

        public string to { get; set; }

        public DateTime timeStamp { get; set; }

        public bool unread { get; set; }

        public ChatMessage()
        {
            unread = false;
        }
    }
}
