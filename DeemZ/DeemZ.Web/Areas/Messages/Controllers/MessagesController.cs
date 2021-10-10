namespace DeemZ.Web.Areas.Messages.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Text.Json;
    using System.Threading.Tasks;
    using DeemZ.Models.FormModels.ChatMessage;
    using DeemZ.Models.ViewModels.ChatMessages;
    using DeemZ.Services.ChatMessageService;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Services.UserServices;
    using DeemZ.Data.Models;
    using DeemZ.Models.DTOs.LiteChat;

    using static DeemZ.Services.EncryptionServices.Base64Service;
    using static DeemZ.Global.WebConstants.Constant.Role;

    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class MessagesController : ControllerBase
    {
        private readonly IChatMessageService chatMessageService;
        private readonly IUserService userService;

        public MessagesController(IChatMessageService chatMessageService, IUserService userService)
        {
            this.chatMessageService = chatMessageService;
            this.userService = userService;
        }

        // GET: api/<Messages>
        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(chatMessageService.GetAllChatMessages<ChatMessageViewModel>());
        }

        // GET api/<Messages>/<id>
        [HttpGet("{id}")]
        public JsonResult Get(string id)
        {
            return new JsonResult(chatMessageService.GetChatMessageById(id));
        }

        // POST api/<MessagesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JsonElement message)
        {
            var parsedMessage = JsonSerializer.Deserialize<ChatMessageInputModel>(message.ToString());
            if (!chatMessageService.CanUserSendMessage(parsedMessage.CourseId, parsedMessage.ApplicationUserId))
            {
                return Unauthorized();
            }
            await chatMessageService.SendChatMessage(parsedMessage);

            return Accepted();
        }

        // DELETE api/<MessagesController>/<id>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (User.IsAdmin())
            {
                await chatMessageService.DeleteChatMessage(id);
                return Accepted();
            }
            return Unauthorized();
        }

        [HttpGet("connect")]
        public async Task<ActionResult<string>> Connect(string courseId, string userId)
        {
            var isAdmin = await userService.IsInRoleAsync(userId, AdminRoleName);

            if (!chatMessageService.CanUserSendMessage(courseId, userId) && ! isAdmin)
            {
                return Unauthorized();
            }

            var user = userService.GetUserById<ApplicationUser>(userId);

            var model = new ConnectDTO
            {
                ApplicationUserId = userId,
                CourseId = courseId,
                IsAdmin = isAdmin,
                ImageUrl = user.ImgUrl
            };

            var json = JsonSerializer.Serialize(model);

            var encryptedString = Encode(json);

            return Ok(encryptedString);
        }
    }
}
