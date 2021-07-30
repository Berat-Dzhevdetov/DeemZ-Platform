namespace DeemZ.Services.UserServices
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DeemZ.Data;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.User;

    public class UserService : IUserService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(DeemZDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        //If the given role doesn't exists it will create it automatically
        public async Task AddUserToRole(string userId, string role)
        {
            var user = GetUserById<ApplicationUser>(userId);

            if (!await roleManager.RoleExistsAsync(role)) await roleManager.CreateAsync(new IdentityRole { Name = role });

            await userManager.AddToRoleAsync(user, role);

            context.SaveChanges();
        }

        public async Task RemoveUserFromRole(string userId, string role)
        {
            var user = GetUserById<ApplicationUser>(userId);

            await userManager.RemoveFromRoleAsync(user, role);

            context.SaveChanges();
        }   

        public async Task EditUser(string userId, EditUserFormModel user)
        {
            var userToEdit = GetUserById<ApplicationUser>(userId);

            context.Attach(userToEdit);


            userToEdit.FirstName = user.FirstName;
            userToEdit.LastName = user.LastName;
            userToEdit.UserName = user.UserName;
            userToEdit.Email = user.Email;
            userToEdit.EmailConfirmed = user.EmailConfirmed;
            userToEdit.LockoutEnd = user.LockoutEndDateUtc;
            userToEdit.PhoneNumber = user.PhoneNumber;

            await context.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllUsers<T>(int page = 1, int quantity = 20)
            => context.Users
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList()
                .Paging(page, quantity);

        public T GetUserById<T>(string uid)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == uid);

            return mapper.Map<T>(user);
        }

        public int GetUserTakenCourses(string uid)
            => context.UserCourses
                .Count(x => x.IsPaid == true && x.UserId == uid);

        public async Task<bool> IsInRole(string userId, string role)
        {
            var user = GetUserById<ApplicationUser>(userId);

            return await userManager.IsInRoleAsync(user, role);
        }

        public bool IsEmailFree(string uid, string email)
            => context.Users.Any(x => x.Email == email && x.Id != uid);

        public bool IsUsernameFree(string uid, string userName)
            => context.Users.Any(x => x.UserName == userName && x.Id != uid);
    }
}
