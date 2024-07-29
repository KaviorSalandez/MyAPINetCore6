using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MyAPINetCore6.Data;
using System;

namespace MyAPINetCore6.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly BookStoreContext _context;
        public NotificationHub(BookStoreContext context)
        {
            _context = context;
        }
        public async Task SendNotificationToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
        public async Task SendNotificationToClient(string message, string userId)
        {
            var hubConnections = await _context.HubConnections.Where(x => x.UserId == userId).ToListAsync();
            foreach (var hubConnection in hubConnections)
            {
                await Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivePersonalNotification", message, userId);
            }
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("OnConnected");
            return base.OnConnectedAsync();
        }

        public override  Task OnDisconnectedAsync(Exception? exception)
        {
            var hubConnection = _context.HubConnections.FirstOrDefault(con => con.ConnectionId == Context.ConnectionId);
            if (hubConnection != null)
            {
                _context.HubConnections.Remove(hubConnection);
                _context.SaveChanges();
            }
            return base.OnDisconnectedAsync(exception);
        }
        public async Task SaveUserConnection(string UserId)
        {
            var connectionId = Context.ConnectionId;
            HubConnection hubConnection = new HubConnection()
            {
                ConnectionId = connectionId,
                UserId = UserId
            };
            _context.HubConnections.Add(hubConnection);
            await _context.SaveChangesAsync();
        }
    }
}
