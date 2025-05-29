using Microsoft.AspNetCore.SignalR;
using SignalrD1.Models;
namespace SignalrD1.Hubs
{
    public class ChatHub:Hub
    {
        ChatContext db;
        public ChatHub(ChatContext db)
        {
            this.db = db; 
        }
        public void sendmessage(ChatMessage mess)
        {            
            db.ChatMessages.Add(mess);
            db.SaveChanges();
            Clients.All.SendAsync("newmessage",mess);
        }
        public async Task joinGroup(string groupName,string name)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var joinMessage = new ChatMessage
            {
                username = name,
                message = $"{name} joined the {groupName} group"
            };
            await Clients.OthersInGroup(groupName).SendAsync("newmessage", joinMessage);
            await Clients.Caller.SendAsync("joinedGroup", groupName, name);

        }
        public async Task sendMessageToGroup(string groupName, ChatMessage mess)
        {
            db.ChatMessages.Add(mess);
            db.SaveChanges();
            await Clients.Group(groupName).SendAsync("newmessage", mess);
        }

        public async Task leaveGroup(string groupName, string name)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            var leaveMessage = new ChatMessage
            {
                username = name,
                message = $"{name} left the {groupName} group"
            };

            await Clients.OthersInGroup(groupName).SendAsync("newmessage", leaveMessage);
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
