﻿namespace DeemZ.Web.Infrastructure
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
    using DeemZ.Services.ExamServices;
    using DeemZ.Services.Question;
    using DeemZ.Services.InformativeMessageServices;
    using DeemZ.Services.CachingService;
    using DeemZ.Services.EmailSender;
    using DeemZ.Services.ChatMessageService;
    using DeemZ.Services.PromoCodeServices;
    using DeemZ.Services.PdfServices;
    using DeemZ.Services.CertificateServices;
    using DeemZ.Services.PartnerServices;
    using DeemZ.Services.IpLoggServices;
    using DeemZ.Services.LocationServices;

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
                mc.AddProfile(new ForumProfile());
                mc.AddProfile(new ReportProfile());
                mc.AddProfile(new UserCourseProfile());
                mc.AddProfile(new ExamProfile());
                mc.AddProfile(new InformativeMessageProfile());
                mc.AddProfile(new ChatMessagesProfile());
                mc.AddProfile(new PromoCodeProfile());
                mc.AddProfile(new CertificateProfile());
                mc.AddProfile(new PartnerProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<Guard>();

            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<ISurveyService, SurveyService>();
            services.AddTransient<IForumService, ForumService>();
            services.AddTransient<ILectureService, LectureService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IResourceService, ResourceService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IExamService, ExamService>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IExcelService, ExcelService>();
            services.AddTransient<IInformativeMessageService, InformativeMessageService>();
            services.AddTransient<IMemoryCachingService, MemoryCachingService>();
            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddTransient<IChatMessageService, ChatMessageService>();
            services.AddTransient<IPromoCodeService, PromoCodeService>();
            services.AddTransient<IPdfService, PdfService>();
            services.AddTransient<ICertificateService, CertificateService>();
            services.AddTransient<IPartnerService, PartnerService>();
            services.AddTransient<IIpLoggerService, IpLoggerService>();
            services.AddTransient<ILocationService, LocationService>();
        }
    }
}