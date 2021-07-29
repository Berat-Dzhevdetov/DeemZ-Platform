namespace DeemZ.Web.Infrastructure
{
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using DeemZ.Services.AutoMapperProfiles;
    using DeemZ.Services;
    using DeemZ.Services.CourseServices;
    using DeemZ.Services.SurveyServices;
    using DeemZ.Services.ForumServices;
    using DeemZ.Services.LectureServices;
    using DeemZ.Services.ReportService;
    using DeemZ.Services.AdminServices;
    using DeemZ.Services.ResourceService;
    using DeemZ.Services.FileService;
    using DeemZ.Services.UserServices;

    public static class ServiceBuilderExtentions
    {
        public static void AddMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new CourseProfile());
                mc.AddProfile(new LectureProfile());
                mc.AddProfile(new DescriptionProfile());
                mc.AddProfile(new ResourceProfile());
                mc.AddProfile(new SurveyProfile());
                mc.AddProfile(new SurveyQuestionProfile());
                mc.AddProfile(new SurveyAnswerProfile());
                mc.AddProfile(new ForumProfile());
                mc.AddProfile(new ReportProfile());
                mc.AddProfile(new UserCourseProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<Guard>();

            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<ISurveyService, SurveyService>();
            services.AddTransient<IForumService, ForumService>();
            services.AddTransient<ILectureService, LectureService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IResourceService, ResourceService>();
            services.AddTransient<IFileServices, FileServices>();
            services.AddTransient<IUserService, UserService>();
        }
    }
}