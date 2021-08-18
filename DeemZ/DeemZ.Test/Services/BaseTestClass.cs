namespace DeemZ.Test.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using DeemZ.Data;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Course;
    using DeemZ.Models.FormModels.Lecture;
    using DeemZ.Services.AutoMapperProfiles;
    using DeemZ.Services.CourseServices;
    using DeemZ.Services.FileService;
    using DeemZ.Services.LectureServices;
    using DeemZ.Services.ReportService;
    using DeemZ.Services.ResourceService;
    using DeemZ.Models.FormModels.Resource;
    using DeemZ.Services.UserServices;
    using DeemZ.Services.ForumServices;
    using DeemZ.Models.FormModels.Forum;

    public abstract class BaseTestClass
    {

        public IReportService reportService;
        public ILectureService lectureService;
        public IResourceService resourceService;
        public IFileService fileService;
        public ICourseService courseService;
        public IUserService userService;
        public IForumService forumService;

        public const string testUserId = "test-user";
        public const string issueDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur efficitur nec magna ac pharetra. Praesent sit amet est felis. Maecenas.";
        public DeemZDbContext context;


        protected BaseTestClass()
        {
        }

        public void SetUpServices()
        {
            var options = new DbContextOptionsBuilder<DeemZDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());

            context = new DeemZDbContext(options.Options);


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
            });

            IMapper mapper = mapperConfig.CreateMapper();

            reportService = new ReportService(context, mapper);

            fileService = new FileService(context);

            userService = new UserService(context, mapper, null, null, courseService, null, resourceService, null);

            resourceService = new ResourceService(context, mapper, fileService);

            lectureService = new LectureService(context, mapper,
                resourceService);

            courseService = new CourseService(context, mapper, lectureService);

            forumService = new ForumService(context, mapper);
        }

        public string SeedCourse()
            => courseService.CreateCourse(new AddCourseFormModel
            {
                Name = "Test course 2021",
                Credits = 15,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                SignUpStartDate = DateTime.UtcNow.AddDays(-2),
                SignUpEndDate = DateTime.UtcNow.AddDays(-1),
                Price = 220
            });

        public string SeedLecture(string courseId)
            => lectureService.AddLectureToCourse(courseId, new AddLectureFormModel
            {
                Name = "Very important test"
            });

        //Seeding non admin user
        public void SeedUser()
            => context.Users.Add(new ApplicationUser
            {
                Id = testUserId,
                UserName = testUserId,
                Email = testUserId
            });

        public string SeedResourceTypes()
        {
            context.ResourceTypes.Add(new ResourceType() { Name = "Youtube link", Icon = "<i class=\"fab fa-youtube\"></i>", IsRemote = true });
            
            context.SaveChanges();

            return context.ResourceTypes.First().Id;
        }

        public string SeedResource()
        {
            var courseId = SeedCourse();

            var lectureId = SeedLecture(courseId);

            var resourceTypeId = SeedResourceTypes();

            var resourceToAdd = new AddResourceFormModel
            {
                Name = "Test",
                Path = "Test-path",
                ResourceTypeId = resourceTypeId
            };

            return resourceService.AddResourceToLecture(lectureId, testUserId, resourceToAdd);
        }

        public string SeedForumTopic(bool addUser = true)
        {
            if (addUser) SeedUser();
            return forumService.CreateTopic(new CreateForumTopicFormModel()
            {
                Title = "Test",
                Description = "Test",
            }, testUserId);
        }
    }
}