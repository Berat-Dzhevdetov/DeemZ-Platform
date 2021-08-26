using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DeemZ.Models.ViewModels.Chat;
using Microsoft.AspNetCore.SignalR;

namespace DeemZ.Web.Hubs
{
    public class ChatHub : Hub
    {
        public string Group { get; set; }

        public ChatHub()
        {
            Group = "public";
        }

        public async Task OnConnect()
        {
            Console.WriteLine("CONNECTED");
            var groupName = Context.Items[Context.ConnectionId];
            if (groupName == null)
            {
                await AddToGroup("public");
                Group = "public";
            }
            else
            {
                Group = Context.Items[Context.ConnectionId]!.ToString();
            }
        }

        public async Task Send(string message)
        {
            await Clients.Group(Group).SendAsync("NewMessage", new Message()
            {
                CreatedOn = DateTime.Now.ToShortDateString(),
                Text = message,
                User = $"{Context.User.Identity!.Name}, from group {Group}"
            });

            //if (this.Context.User.IsInRole(GlobalConstants.DoctorRoleName))
            //var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public async Task AddToAdminGroup() => await AddToGroup("admins");
        public async Task RemoveFromAdminGroup() => await RemoveFromGroup("admins");

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            Context.Items[Context.ConnectionId] = groupName;
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            Context.Items.Remove(Context.ConnectionId);
        }
    }
}
