﻿using Microsoft.AspNetCore.SignalR;

namespace Eihal.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            //await Clients.User(user).SendAsync("ReceiveMessage", user, message);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public Task SendMessageToGroup(string groupName, string message)
        {
            return Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId}: {message}");
        }

        public override async Task OnConnectedAsync()
        {
            var userName = Context.User.Identity.Name;
            //ConnectionsDependencyInjectionExtensions.AddConnections()
            //await Groups.AddToGroupAsync(userName, "SignalR Users");
            if (!string.IsNullOrEmpty(userName))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userName);
            }
            await base.OnConnectedAsync();
        }
    }
}