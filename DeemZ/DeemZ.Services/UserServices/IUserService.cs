namespace DeemZ.Services.UserServices
{
    using DeemZ.Models.FormModels.User;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserService
    {
        IEnumerable<T> GetAllUsers<T>(int page = 1, int quantity = 20);
        int GetUserTakenCourses(string uid);
        T GetUserById<T>(string uid);
        Task<bool> IsInRole(string userId, string role);
        Task EditUser(string userId, EditUserFormModel user);
        Task AddUserToRole(string userId, string role);
        Task RemoveUserFromRole(string userId, string role);
        bool IsEmailFree(string uid, string email);
        bool IsUsernameFree(string uid, string userName);
    }
}