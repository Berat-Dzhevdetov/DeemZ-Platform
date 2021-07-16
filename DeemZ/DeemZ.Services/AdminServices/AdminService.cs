namespace DeemZ.Services.AdminServices
{
    using AutoMapper;
    using DeemZ.Data;
    using DeemZ.Models.ViewModels.Administration;
    using System;
    using System.Linq;

    public class AdminService : IAdminService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public AdminService(IMapper mapper, DeemZDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

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
    }
}