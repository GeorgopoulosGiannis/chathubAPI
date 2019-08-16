
using chathubAPI.Helpers;
using chathubAPI.Models;
using chathubAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Hubs
{

    public class User
    {

        public string Name { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }

    [Authorize]
    public class ChatHub : Hub
    {


        private readonly static ConnectionMapping<string> _connections =
               new ConnectionMapping<string>();
        private readonly IUserRepo _userRepo;
        public ChatHub(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async void SendPrivateMessage(ChatMessage message)
        {

            string userId = Context.UserIdentifier;
            string to = GetUserIdFromEmail(message.to);
            string from = GetUserEmailFromId(userId);
            message.from = from;
            if (message.to != null)
            {
                foreach (var connectionId in _connections.GetConnections(to))
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
                }

            }
        }

        public void SendToAll(ChatMessage message)
        {
            Clients.All.SendAsync("sendToAll", message);
        }

        public override Task OnConnectedAsync()
        {

            string userId = Context.UserIdentifier;
            _connections.Add(userId, Context.ConnectionId);

            Clients.All.SendAsync("SendOnlineConnections", this.createConnectedList(_connections.GetKeys()));
            Clients.All.SendAsync("SendConnections", _userRepo.GetAllUsersEmails());
            this.SendToAll(new ChatMessage() { from = this.GetUserEmailFromId(userId), message = "hello" });
            return base.OnConnectedAsync();
        }


        public override Task OnDisconnectedAsync(Exception exception)
        {
            string userId = Context.UserIdentifier;

            _connections.Remove(userId, Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        public string GetUserIdFromEmail(string email)
        {
            return _userRepo.GetUserIdFromEmail(email);
        }
        public string GetUserEmailFromId(string userId)
        {
            return _userRepo.GetUserEmailFromId(userId);
        }
        public List<string> createConnectedList(Dictionary<string, HashSet<string>>.KeyCollection keys)
        {
            List<string> list = new List<string>();
            foreach (var key in keys)
            {
               list.Add(GetUserEmailFromId(key));
            }
            return list;
        }

    }
}
