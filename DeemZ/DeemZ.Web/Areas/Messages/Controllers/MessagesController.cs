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
    using DeemZ.Services.CourseServices;

    using static DeemZ.Services.EncryptionServices.Base64Service;
    using static DeemZ.Global.WebConstants.Constant.Role;
    using static DeemZ.Data.DataConstants.User;
    using System.Linq;

    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class MessagesController : ControllerBase
    {
        private readonly IChatMessageService chatMessageService;
        private readonly IUserService userService;
        private readonly ICourseService courseService;

        public MessagesController(IChatMessageService chatMessageService, IUserService userService, ICourseService courseService)
        {
            this.chatMessageService = chatMessageService;
            this.userService = userService;
            this.courseService = courseService;
        }

        // GET: api/<Messages>
        [HttpGet]
        public JsonResult Get()
        {
            var allMessages = chatMessageService.GetAllChatMessages<ChatMessageViewModel>();

            allMessages.Select(x =>
                    x.ApplicationUserImgUrl = GetUserImageLink(x.ApplicationUserImgUrl)
                ).ToList();

            return new(allMessages);
        }

        // GET api/<Messages>/<id>
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var message = chatMessageService.GetChatMessageById<ChatMessageViewModel>(id);

            if (message == null)
                return NotFound();

            message.ApplicationUserImgUrl = GetUserImageLink(message.ApplicationUserImgUrl);

            return new JsonResult(message);
        }

        [HttpGet("GetCourseMessages/{courseId}")]
        public IActionResult GetCourseMessages(string courseId)
        {
            var messages = chatMessageService.GetChatMessagesByCourse<ChatMessageViewModel>(courseId);

            if (messages == null)
                return NotFound();

            return new JsonResult(messages);
        }

        // POST api/<MessagesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JsonElement message)
        {
            var parsedMessage = JsonSerializer.Deserialize<ChatMessageInputModel>(message.ToString());
            var isAdmin = await userService.IsInRoleAsync(parsedMessage.ApplicationUserId, AdminRoleName);

            if (!chatMessageService.CanUserSendMessage(parsedMessage.CourseId, parsedMessage.ApplicationUserId)
                && !isAdmin)
                return Unauthorized();

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

        // GET api/<MessagesController>/<courseId>/<applicationUserId>
        [HttpGet("connect/{courseId}/{applicationUserId}")]
        public async Task<ActionResult<string>> Connect(string courseId, string applicationUserId)
        {
            if (!userService.GetUserById(applicationUserId)) return NotFound();
            if (!courseService.GetCourseById(courseId)) return NotFound();

            var isAdmin = await userService.IsInRoleAsync(applicationUserId, AdminRoleName);

            if (!chatMessageService.CanUserSendMessage(courseId, applicationUserId) && !isAdmin)
                return Unauthorized();

            var user = userService.GetUserById<ApplicationUser>(applicationUserId);

            var course = courseService.GetCourseById<Course>(courseId);

            var model = new ConnectDTO
            {
                ApplicationUserId = applicationUserId,
                CourseId = courseId,
                IsAdmin = isAdmin,
                ApplicationUserImageUrl = GetUserImageLink(user.ImgUrl),
                UserName = user.UserName,
                CourseName = course.Name,
            };

            var json = JsonSerializer.Serialize(model);

            var encryptedString = Encode(json);

            return new JsonResult(encryptedString);
        }

        private string GetUserImageLink(string url)
            => url == DefaultProfilePictureUrl ? $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{DefaultProfilePictureUrl}" : url;
    }
}
