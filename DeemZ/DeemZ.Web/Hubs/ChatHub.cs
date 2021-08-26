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
        }

        public async Task OnConnect(string groupId)
        {
            await AddToGroup(groupId);
            Group = groupId;
        }

        public async Task Send(string message, string groupId)
        {
            await Clients.Group(groupId).SendAsync("NewMessage", new Message()
            {
                CreatedOn = DateTime.Now.ToShortDateString(),
                Text = message,
                User = $"{Context.User!.Identity!.Name}, from group {groupId}"
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
