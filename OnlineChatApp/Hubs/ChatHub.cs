using Microsoft.AspNetCore.SignalR;
using OnlineChatApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChatApp.Hubs
{
    public class ChatHub: Hub
    {
        private readonly OnlineChatDbContext _dbContext;
        public ChatHub(OnlineChatDbContext onlineChatDb)
        {
            _dbContext = onlineChatDb;
        }

        public async override Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("updateConnectionId", Context.ConnectionId);
        }

        

        public async Task SendMessage(string user, string message)
        {
            
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
