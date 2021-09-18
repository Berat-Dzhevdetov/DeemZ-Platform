namespace DeemZ.Services.AdminServices
{
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper.QueryableExtensions;
    using System.Threading.Tasks;
    using DeemZ.Data;
    using DeemZ.Models.ViewModels.Administration;
    using DeemZ.Services.CourseServices;
    using DeemZ.Services.UserServices;
    using DeemZ.Global.Extensions;

    using static DeemZ.Global.WebConstants.Constant;

    public class AdminService : IAdminService
    {
        private readonly DeemZDbContext context;
        private readonly ICourseService courseService;
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public AdminService(IMapper mapper, DeemZDbContext context, ICourseService courseService, IUserService userService)
        {
            this.mapper = mapper;
            this.context = context;
            this.courseService = courseService;
            this.userService = userService;
        }

        public IEnumerable<T> GetUserCourses<T>(int page = 1, int quantity = 20)
            => context.UserCourses
                .Include(x => x.Course)
                .Include(x => x.User)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .Paging(page, quantity)
                .ToList();

        public IEnumerable<T> GetAllCourses<T>(int page = 1, int quantity = 20)
             => context.Courses
                .OrderByDescending(x => x.StartDate)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .Paging(page, quantity)
                .ToList();

        public AdministrationIndexViewModel GetIndexPageInfo()
        {
            var lastMonthDay = DateTime.Now.AddDays(-30);

            var model = new AdministrationIndexViewModel()
            {
                TotalCourses = context.Courses.Count(),
                UsersCount = context.Users.Count(),
                UsersSignedUpThisMonth = context.Users
                    .Where(x => x.CreatedOn >= lastMonthDay).Count(),
                MoneyEarnedThisMonth = GetMoneyEarntByDay(lastMonthDay)
            };

            return model;
        }

        private decimal GetMoneyEarntByDay(DateTime prevsDate)
            => context.UserCourses
                .Include(x => x.Course)
                .Where(x => x.PaidOn >= prevsDate && x.IsPaid)
                .ToList()
                .Where(x => !IsInRole(x.UserId, Role.AdminRoleName))
                .Sum(x => x.Course.Price);

        private bool IsInRole(string uid,string role)
        {
            var result = Task.Run(async () =>
            {
                return await userService.IsInRoleAsync(uid, role);
            });
            return result.Result;
        }

        public int GetUserCoursesCount()
            => courseService.GetUserCoursesCount();

        public int GetUserSignUpForCourse(string cid)
            => context.UserCourses
                .GroupBy(x => x.CourseId)
                .Select(x => x.Count())
                .First();

        public int GetUserSignedUpForCourse(string cid)
            => context.UserCourses.Count(x => x.CourseId == cid);
    }
}