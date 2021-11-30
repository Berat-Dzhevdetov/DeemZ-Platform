namespace DeemZ.Web.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DeemZ.Models.ViewModels.Chat;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Services.UserServices;
    using DeemZ.Data.Models;

    using static DeemZ.Global.WebConstants.Constant;
    using DeemZ.Services.FileService;

    public class ChatHub : Hub
    {
        private readonly IUserService userService;
        private readonly IFileService fileService;

        public ChatHub(IUserService userService, IFileService fileService)
        {
            this.userService = userService;
            this.fileService = fileService;
        }

        public string Group { get; set; }

        public async Task OnConnect(string groupId)
        {
            await AddToGroup(groupId);
            Group = groupId;
        }

        public async Task Send(string message, string groupId, string[] imageUrls)
        {
            var userId = Context.User.GetId();
            var user = userService.GetUserById<ApplicationUser>(userId);

            DecodeAndUploadImages(imageUrls.Where(x => !String.IsNullOrEmpty(x)).ToArray());

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

        private void DecodeAndUploadImages(string[] imageUrls)
        {
            foreach (var imageUrl in imageUrls)
            {
                var imageBytes = Convert.FromBase64String(imageUrl);
                (string url, string publicId) = fileService.UploadFileBytesToCloud(imageBytes, Guid.NewGuid().ToString());
            }
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
