using DatingApp.WebApi.Infrastracture.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.WebApi.SignalR
{
    [Authorize]
    public class PresenceHub(PresenceTracker presenceTracker) : Hub
    {
        public override async Task OnConnectedAsync()
        {
          var isOnline =  await presenceTracker.UserConnected(Context.User.GetUserName(), Context.ConnectionId);
            if (isOnline)
               await Clients.Others.SendAsync("UserIsOnline" , Context.User.GetUserName());

            var onlineusers = await presenceTracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", onlineusers);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
           var isOffline =  await presenceTracker.UserDisConnected(Context.User.GetUserName(), Context.ConnectionId);

            if (isOffline)
                await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUserName());

            //var onlineusers = await presenceTracker.GetOnlineUsers();
            //await Clients.Caller.SendAsync("GetOnlineUsers", onlineusers);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
