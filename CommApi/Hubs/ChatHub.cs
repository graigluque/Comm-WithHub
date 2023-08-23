using CommApi.Data;
using Microsoft.AspNetCore.SignalR;

namespace CommApi.Hubs
{

    public class ChatHub : Hub
    {
        public ChatHub()
        {

        }

        public override async Task OnConnectedAsync()
        {
            var sender = Context.UserIdentifier;

            Console.WriteLine("[Custom msg]: OnConnectedAsync ...");
            Console.WriteLine("[Custom msg]: Connection ID: " + Context.ConnectionId);
            // //  ToDo: Push the latest session (Connected) information to the user.

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var sender = Context.UserIdentifier;

            Console.WriteLine("[Custom msg]: OnDisconnectedAsync ...");
            Console.WriteLine("[Custom msg]: Connection ID: " + Context.ConnectionId);

            // //  ToDo: Push the latest session (Disconnected) information to the user.

            await base.OnDisconnectedAsync(exception);
        }

        public async Task BroadcastMessage(object message)
        {
            Console.WriteLine("BroadcastMessage sending ...");

            await Clients.All.SendCoreAsync("public", new object[] { message });
        }

        public async Task NotifyToGroup(string method, object message)
        {
            Console.WriteLine("[Custom Msg]: NotifyToGroup sending ...");

            await Clients.All.SendCoreAsync(method, new object[] { message });
        }

    }
}
