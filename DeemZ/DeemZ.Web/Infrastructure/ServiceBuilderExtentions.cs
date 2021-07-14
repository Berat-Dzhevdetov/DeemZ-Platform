namespace DeemZ.Web.Infrastructure
{
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using DeemZ.Services.AutoMapperProfiles;

    public static class ServiceBuilderExtentions
    {
        public static void AddMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new CourseProfile());
                mc.AddProfile(new LectureProfile());
                mc.AddProfile(new ResourceTypeProfile());
                mc.AddProfile(new DescriptionProfile());
                mc.AddProfile(new ResourceProfile());
                mc.AddProfile(new SurveyProfile());
                mc.AddProfile(new SurveyQuestionProfile());
                mc.AddProfile(new SurveyAnswerProfile());
                mc.AddProfile(new ForumProfile());
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
        }
    }
}