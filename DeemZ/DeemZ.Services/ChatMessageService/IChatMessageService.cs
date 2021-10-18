namespace DeemZ.Services.ChatMessageService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeemZ.Models.FormModels.ChatMessage;

    public interface IChatMessageService
    {
        Task<string> SendChatMessage(ChatMessageInputModel inputModel);

        bool CanUserSendMessage(string cid, string uid);

        Task DeleteChatMessage(string mid);

        IEnumerable<T> GetChatMessagesByCourse<T>(string cid);
        
        IEnumerable<T> GetAllChatMessages<T>();

        T GetChatMessageById<T>(string mid);

        bool IfExists(string mid);
    }
}
