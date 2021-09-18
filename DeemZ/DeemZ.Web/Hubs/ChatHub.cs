namespace DeemZ.Web.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System;
    using System.Threading.Tasks;
    using DeemZ.Models.ViewModels.Chat;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Services.UserServices;
    using DeemZ.Data.Models;

    using static DeemZ.Global.WebConstants.Constant;

    public class ChatHub : Hub
    {
        private readonly IUserService userService;

        public ChatHub(IUserService userService) 
            => this.userService = userService;

        public string Group { get; set; }

        public async Task OnConnect(string groupId)
        {
            await AddToGroup(groupId);
            Group = groupId;
        }

        public async Task Send(string message, string groupId)
        {
            var userId = Context.User.GetId();
            var user = userService.GetUserById<ApplicationUser>(userId);

            await Clients.Group(groupId).SendAsync("NewMessage", new Message()
            {
                CreatedOn = DateTime.Now.ToString(DateTimeFormat),
                Text = message,
                SenderId = user.Id,
                SenderName = user.UserName,
                SenderImg = user.ImgUrl,
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
