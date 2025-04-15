using Microsoft.AspNetCore.SignalR;

namespace Trello.WebSocket
{
    public class CardHub : Hub
    {
        public async Task NotifyCardMoved(int cardId, string newStatus)
        {
            await Clients.All.SendAsync("CardMoved", cardId, newStatus);
        }
    }

}
