namespace DeemZ.Services.UserServices
{
    using System.Collections.Generic;

    public interface IUserService
    {
        IEnumerable<T> GetAllUsers<T>(int page = 1, int quantity = 20);
        int GetUserTakenCourses(string uid);
    }
}