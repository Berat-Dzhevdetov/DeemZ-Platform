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
    using DeemZ.Web.Filters;

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
        public ActionResult<JsonResult> Get()
            => new JsonResult(chatMessageService.GetAllChatMessages<ChatMessageViewModel>());

        // GET api/<Messages>/<id>
        [HttpGet("{id}")]
        public ActionResult<JsonResult> Get(string id) => new JsonResult(chatMessageService.GetChatMessageById(id));

        // POST api/<MessagesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JsonElement message)
        {
            var parsedMessage = JsonSerializer.Deserialize<ChatMessageInputModel>(message.ToString());
            var isAdmin = User.IsAdmin();

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

        // GET api/<MessagesController>/connect
        [HttpGet("connect")]
        public async Task<ActionResult<string>> Connect(string courseId, string applicationUserId)
        {
            var isAdmin = await userService.IsInRoleAsync(applicationUserId, AdminRoleName);

            if (!chatMessageService.CanUserSendMessage(courseId, applicationUserId) && ! isAdmin)
                return Unauthorized();

            var user = userService.GetUserById<ApplicationUser>(applicationUserId);

            var course = courseService.GetCourseById<Course>(courseId);

            var model = new ConnectDTO
            {
                ApplicationUserId = applicationUserId,
                CourseId = courseId,
                IsAdmin = isAdmin,
                ApplicationUserImageUrl = user.ImgUrl == DefaultProfilePictureUrl ? $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{DefaultProfilePictureUrl}" : user.ImgUrl,
                UserName = user.UserName,
                CourseName = course.Name,
            };

            var json = JsonSerializer.Serialize(model);

            var encryptedString = Encode(json);

            return Ok(encryptedString);
        }
    }
}
