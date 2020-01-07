using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using chathubAPI.Models;
using chathubAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace chathubAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        readonly IUserRepo _userRepo;
        readonly IMessagesRepo _messagesRepo;
        readonly UserManager<IdentityUser> _userManager;

        public MessagesController(IMessagesRepo messagesRepo, UserManager<IdentityUser> userManager, IUserRepo userRepo)
        {
            _messagesRepo = messagesRepo;
            _userRepo = userRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessageHistory(string to, int currentPage = 1)
        {
            try
            {

                string from = _userRepo.GetUserEmailFromId(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                List<ChatMessage> fromTo = _messagesRepo.GetMessageHistory(from, to, currentPage);
                List<ChatMessage> toFrom = _messagesRepo.GetMessageHistory(to, from, currentPage);
                var messageHistory = fromTo.Concat(toFrom).ToList();
                messageHistory = messageHistory.OrderByDescending(x => x.timeStamp).ToList();
                foreach (var message in messageHistory)
                {
                    message.unread = false;
                    _messagesRepo.UpdateRecord(message);
                }
                return Ok(messageHistory);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        [HttpGet("unread")]
        public async Task<IActionResult> GetUnreadMessages()
        {
            try
            {
                string to = _userRepo.GetUserEmailFromId(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var messages = _messagesRepo.GetUnreadMessages(to);

                return Ok(messages);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Read(ChatMessage message)
        {
            try
            {
                ChatMessage msg = _messagesRepo.FindMessage(message);
                if (msg != null)
                {
                    msg.unread = false;
                    _messagesRepo.UpdateRecord(msg);
                    _messagesRepo.Save();

                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}