using ChatApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Hubs
{
    [Authorize]
    
    public class ChatHub:Hub

    {
        private readonly AppDbContext appDbContext;

        public ChatHub(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public override Task OnConnectedAsync()
             
        {
            
            User user = this.appDbContext.Users.FirstOrDefault(U => U.UserName == Context.User.Identity.Name);
          
            
            user.connrctionId = Context.ConnectionId;
            this.appDbContext.Users.Update(user);
            this.appDbContext.SaveChanges();
            
            return base.OnConnectedAsync();
            
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public Task SendToMe(string sender, string reciever, string message)
        {
            //User user = this.appDbContext.Users.FirstOrDefault(U => U.UserName == reciever);
            return Clients.Caller.SendAsync("ReceiveMessage", sender, message);
        }

        public async Task SendMessageToGroup(string sender, string reciever, string Message)
        {
            
            User Sender = this.appDbContext.Users.FirstOrDefault(x => x.UserName == sender);
            User Reciever = this.appDbContext.Users.FirstOrDefault(x => x.UserName == reciever);
            Messages message = new Messages()
            {
                message = Message,
                UserSender = Sender,
                Reciever = Reciever,

            };

            this.appDbContext.Messages.Add(message);
            this.appDbContext.SaveChanges();
           
            await Clients.Client(Reciever.connrctionId).SendAsync("ReceiveMessage", sender, Message);
        }
    }
}
