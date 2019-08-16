using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using chathubAPI.Models;
using chathubAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace chathubAPI.Controllers
{
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
        public async Task<IActionResult> GetMessageHistory( string to, int currentPage = 1)
        {

             string from = User.FindFirst(ClaimTypes.NameIdentifier).Value;
           // string from = "asdf@asdf.gr";
            to = GetUserIdFromEmail(to);
            List<ChatMessage> fromTo = _messagesRepo.GetMessageHistory(from, to, currentPage);
            List<ChatMessage> toFrom = _messagesRepo.GetMessageHistory(to, from, currentPage);
            var messageHistory = fromTo.Concat(toFrom).ToList();
            messageHistory.OrderBy(x => x.TimeStamp);
            return Ok(messageHistory);

        }

        private string GetUserIdFromEmail(string email)
        {
            return _userRepo.GetUserIdFromEmail(email);
        }
        private string GetUserEmailFromId(string userId)
        {
            return _userRepo.GetUserEmailFromId(userId);
        }
    }
}