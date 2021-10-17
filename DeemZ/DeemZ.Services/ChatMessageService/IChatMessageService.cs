namespace DeemZ.Services.ChatMessageService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeemZ.Models.FormModels.ChatMessage;

    public interface IChatMessageService
    {
        Task<string> SendChatMessage(ChatMessageInputModel inputModel);

        bool CanUserSendMessage(string courseId, string userId);

        Task DeleteChatMessage(string messsageId);

        IEnumerable<T> GetChatMessagesByCourse<T>(string courseId);
        
        IEnumerable<T> GetAllChatMessages<T>();

        T GetChatMessageById<T>(string messageId);
    }
}
