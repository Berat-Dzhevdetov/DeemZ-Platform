﻿namespace DeemZ.Services.UserServices
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
    using DeemZ.Models.ViewModels.User;
    using DeemZ.Services.CourseServices;
    using DeemZ.Services.SurveyServices;
    using DeemZ.Models.ViewModels.Course;
    using DeemZ.Models.ViewModels.Surveys;
    using DeemZ.Models.ViewModels.Resources;
    using DeemZ.Services.ResourceService;

    public class UserService : IUserService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ICourseService courseService;
        private readonly ISurveyService surveyService;
        private readonly IResourceService resourceService;

        public UserService(DeemZDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ICourseService courseService, ISurveyService surveyService, IResourceService resourceService)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.courseService = courseService;
            this.surveyService = surveyService;
            this.resourceService = resourceService;
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

        public IndexUserViewModel GetIndexInformaiton(string uid)
            => new IndexUserViewModel()
            {
                Credits = courseService.GetUserCredits(uid),
                Courses = courseService.GetUserCurrentCourses<IndexCourseViewModel>(uid, true),
                Surveys = surveyService.GetUserCurrentCourseSurveys<IndexSurveyViewModel>(uid),
                Resources = resourceService.GetUserResources<IndexResourceViewModel>(uid),
                SignUpCourses = courseService.GetCoursesForSignUp<IndexSignUpForCourseViewModel>()
            };
    }
}