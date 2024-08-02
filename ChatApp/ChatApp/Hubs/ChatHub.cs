using ChatApp.Context;
using ChatApp.Models;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public sealed class ChatHub(ApplicationDbContext context) : Hub
    {
        public static Dictionary<string, int> Users = new();
        public async Task Connect(int userId)
        {
            Users.Add(Context.ConnectionId, userId);
            User? user = await context.Users.FindAsync(userId);
            if (user is not null)
            {
                user.Status = "Online";
                await context.SaveChangesAsync();

                await Clients.All.SendAsync("Users", user);
            };
        }
        public override async Task OnDisconnectedAsync(Exception? exception) // token süresi bitince signalR dan da disconnect atması lazım
        {
            int userId;
            Users.TryGetValue(Context.ConnectionId, out userId);
            Users.Remove(Context.ConnectionId);
            User? user = await context.Users.FindAsync(userId);
            if (user is not null)
            {
                user.Status = "Offline";
                await context.SaveChangesAsync();

                await Clients.All.SendAsync("Users", user);

            }

        }
    }
}