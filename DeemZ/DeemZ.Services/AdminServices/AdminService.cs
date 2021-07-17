namespace DeemZ.Services.AdminServices
{
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DeemZ.Data;
    using DeemZ.Models.ViewModels.Administration;
    using DeemZ.Services.CourseServices;
    using Microsoft.EntityFrameworkCore;

    public class AdminService : IAdminService
    {
        private readonly DeemZDbContext context;
        private readonly ICourseService courseService;
        private readonly IMapper mapper;

        public AdminService(IMapper mapper, DeemZDbContext context, ICourseService courseService)
        {
            this.mapper = mapper;
            this.context = context;
            this.courseService = courseService;
        }

        public IEnumerable<T> GetUserCourses<T>(int page = 1, int quantity = 20)
            => context.UserCourses
                .Include(x => x.Course)
                .Include(x => x.User)
                .Select(x => mapper.Map<T>(x))
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
                MoneyEarnedThisMonth = GetLastMonthMoneyEarned(lastMonthDay)
            };

            return model;
        }

        private decimal GetLastMonthMoneyEarned(DateTime prevsDate)
            => context.UserCourses.Where(x => x.PaidOn >= prevsDate)
                .Sum(x => x.Course.Price);

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