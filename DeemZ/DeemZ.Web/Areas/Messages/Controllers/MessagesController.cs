namespace DeemZ.Web.Areas.Messages.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Text.Json;
    using System.Threading.Tasks;
    using DeemZ.Models.FormModels.ChatMessage;
    using DeemZ.Models.ViewModels.ChatMessages;
    using DeemZ.Services.ChatMessageService;
    using DeemZ.Web.Infrastructure;

    using static DeemZ.Services.EncryptionServices.Base64Service;

    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class MessagesController : ControllerBase
    {
        private readonly IChatMessageService chatMessageService;

        public MessagesController(IChatMessageService chatMessageService)
        {
            this.chatMessageService = chatMessageService;
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
    }
}
