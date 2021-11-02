namespace DeemZ.Services.UserServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeemZ.Models.FormModels.User;
    using DeemZ.Models.ViewModels.User;

    public interface IUserService
    {
        IEnumerable<T> GetAllUsers<T>(int page = 1, int quantity = 20);
        int GetUserTakenCourses(string uid);
        T GetUserById<T>(string uid);
        bool GetUserById(string uid);
        Task<bool> IsInRoleAsync(string uid, string role);
        Task EditUser(string uid, EditUserFormModel user);
        Task AddUserToRole(string uid, string role);
        Task RemoveUserFromRole(string uid, string role);
        bool IsEmailFree(string uid, string email);
        bool IsUsernameFree(string uid, string userName);
        IndexUserViewModel GetIndexInformaiton(string uid, bool isAdmin);
        bool GetUserByUserName(string username);
        string GetUserIdByUserName(string username);
        void SetProfileImg(string id, string url, string publidId);
        void DeleteUserProfileImg(string userId);
    }
}