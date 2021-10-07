using DeemZ.Data.Models;
using DeemZ.Models.FormModels.ChatMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeemZ.Services.ChatMessageService
{
    public interface IChatMessageService
    {
        Task<string> SendChatMessage(ChatMessageInputModel inputModel);

        bool CanUserSendMessage(string courseId, string userId);

        Task DeleteChatMessage(string messsageId);

        IEnumerable<T> GetChatMessagesByCourse<T>(string courseId);
        
        IEnumerable<T> GetAllChatMessages<T>();

        ChatMessage GetChatMessageById(string messageId);
    }
}
