using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRDemoCore.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly NotificationTicker _ticker;

        public NotificationHub(NotificationTicker ticker)
        {
            _ticker = ticker;
        }

        public IEnumerable<string> GetAllStocks()
        {
            return _ticker.GetAllNotifications();
        }

        public async Task SendNotiFication(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveSingleNotification", message);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
