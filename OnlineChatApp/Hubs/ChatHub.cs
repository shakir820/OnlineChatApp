using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
        public ChatHub(OnlineChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async override Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("updateConnectionId", Context.ConnectionId);
            

        }



        public async override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(a => a.ConnectionId == Context.ConnectionId);
                user.ConnectionId = null;
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }
          

        }




        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
