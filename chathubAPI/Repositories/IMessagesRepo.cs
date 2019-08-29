using chathubAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public interface IMessagesRepo
    {
        List<ChatMessage> GetMessageHistory(string from,string to,int currentPage);

        void AddMessage(ChatMessage message);

        void Save();

        List<ChatMessage> GetUnreadMessages(string to);

        void UpdateRecord(ChatMessage message);

        ChatMessage FindMessage(ChatMessage message);
    }
}
