using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chathubAPI.DATA;
using chathubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace chathubAPI.Repositories
{
    public class MessagesRepo : IMessagesRepo
    {
        readonly ApplicationDbContext _dbContext;
        public MessagesRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddMessage(ChatMessage message)
        {
            try
            {
                _dbContext.Μessages.Add(message);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ChatMessage> GetMessageHistory(string from, string to, int currentPage = 1)
        {
            int start = (currentPage - 1) * Helpers.Constants.MESSAGES_PER_PAGE;
            try
            {
                var messages = _dbContext.Μessages.Where(x => x.from == from && x.to == to).Skip(start).Take(Helpers.Constants.MESSAGES_PER_PAGE).ToList();

                return messages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public void Save()
        {
            _dbContext.SaveChangesAsync();
        }
        public void UpdateRecord(ChatMessage message)
        {
          var msg = _dbContext.Μessages.Where(x => x.Id == message.Id).FirstOrDefault();
            msg.unread = false;
            _dbContext.SaveChanges();
        }
        public List<ChatMessage> GetUnreadMessages(string to)
        {
            try
            {
                return _dbContext.Μessages.Where(x => x.to == to && x.unread == true).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
