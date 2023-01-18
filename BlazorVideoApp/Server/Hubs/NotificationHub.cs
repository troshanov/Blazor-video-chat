using BlazorVideoApp.Shared;
using Microsoft.AspNetCore.SignalR;

namespace BlazorVideoApp.Server.Hubs
{
    public class NotificationHub : Hub
    {
        public Task RoomsUpdated(string room) =>
            Clients.All.SendAsync(HubEndpoint.RoomsUpdated, room);
    }
}
