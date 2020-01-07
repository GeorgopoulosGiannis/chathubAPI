
using chathubAPI.DTO;
using chathubAPI.Helpers;
using chathubAPI.INTERFACES;
using chathubAPI.Models;
using chathubAPI.Repositories;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMessagesRepo _messagesRepo;
        private readonly IFcmTokenRepo _fcmTokenRepo;
        public ChatHub(IUserRepo userRepo, IMessagesRepo messagesRepo, IFcmTokenRepo fcmTokenRepo)
        {
            _userRepo = userRepo;
            _messagesRepo = messagesRepo;
            _fcmTokenRepo = fcmTokenRepo;
        }

        public async Task SendPrivateMessage(ChatMessage message)
        {

            string userId = Context.UserIdentifier;
            string to = GetUserIdFromEmail(message.to);
            string from = GetUserEmailFromId(userId);
            message.from = from;
            message.timeStamp = DateTime.UtcNow;
            message.unread = true;
            if (message.to != null)
            {

                foreach (var connectionId in _connections.GetConnections(to))
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
                }

                try
                {
                    _messagesRepo.AddMessage(message);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            try
            {
                var toFcmTokens = await _fcmTokenRepo.GetUserTokens(to);
                // See documentation on defining a message payload.
                Dictionary<int, Task<string>> dictionary = new Dictionary<int, Task<string>>();
                for (int i = 0; i < toFcmTokens.Count(); i++)
                {
                    var msg = new Message()
                    {
                        Notification = new Notification
                        {
                            Title = "New message from" + from,
                            Body = message.message
                        },
                        Token = toFcmTokens[i].Token
                    };
                    // Send a message to the device corresponding to the provided
                    // registration token.

                    dictionary[i] = FirebaseMessaging.DefaultInstance.SendAsync(msg);
                }
                await Task.WhenAll(dictionary.Values);
                foreach (var entry in dictionary)
                {
                    Console.WriteLine(entry.Key.ToString() + ' ' + entry.Value);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void MarkMessageAsRead(ChatMessage message)
        {
            message.unread = false;
            _messagesRepo.Save();

        }
        public void SendToAll(ChatMessage message)
        {
            Clients.All.SendAsync("sendToAll", message);
        }

        public override Task OnConnectedAsync()
        {

            string userId = Context.UserIdentifier;
            _connections.Add(userId, Context.ConnectionId);
            //_messagesRepo.GetUnreadMessages(GetUserEmailFromId(userId));
            Clients.All.SendAsync("SendOnlineConnections", this.createConnectedList(_connections.GetKeys()));
            Clients.All.SendAsync("SendConnections", _userRepo.GetAllUsersEmails());
            this.SendToAll(new ChatMessage() { from = this.GetUserEmailFromId(userId), message = "hello" });
            return base.OnConnectedAsync();
        }


        public override Task OnDisconnectedAsync(Exception exception)
        {
            string userId = Context.UserIdentifier;

            _connections.Remove(userId, Context.ConnectionId);
            Clients.All.SendAsync("SendOnlineConnections", this.createConnectedList(_connections.GetKeys()));

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
            foreach (var key in keys.ToList())
            {
                list.Add(GetUserEmailFromId(key));
            }
            return list;
        }

    }
}
