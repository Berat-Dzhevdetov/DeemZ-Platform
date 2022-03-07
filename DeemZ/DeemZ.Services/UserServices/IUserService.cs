namespace DeemZ.Services.UserServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeemZ.Models.FormModels.User;
    using DeemZ.Models.ViewModels.User;

    public interface IUserService
    {
        IEnumerable<T> GetAllUsers<T>(string searchTerm = null, int page = 1, int quantity = 20);
        Task<int> GetUserTakenCourses(string uid);
        Task<T> GetUserById<T>(string uid);
        bool GetUserById(string uid);
        Task<bool> IsInRoleAsync(string uid, string role);
        Task EditUser(string uid, EditUserFormModel user);
        Task AddUserToRole(string uid, string role);
        Task RemoveUserFromRole(string uid, string role);
        bool IsEmailFree(string uid, string email);
        bool IsUsernameFree(string uid, string userName);
        Task<IndexUserViewModel> GetIndexInformaiton(string uid, bool isAdmin);
        bool GetUserByUserName(string username);
        Task<string> GetUserIdByUserName(string username);
        Task SetProfileImg(string id, string url, string publidId);
        Task DeleteUserProfileImg(string userId);
        Task<DetailsUserInformationViewModel> GetUserInformation(string uid);
        Task<bool> UserExists(string uid);
    }
}