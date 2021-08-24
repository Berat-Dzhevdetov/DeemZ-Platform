using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using DeemZ.Services;
using DeemZ.Services.SurveyServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;

namespace DeemZ.Test.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
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
    using DeemZ.Models.FormModels.Description;
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
        public ISurveyService surveyService;
        public IForumService forumService;
        public Guard guard = new Guard();

        public const string testUserId = "test-user";
        public const string issueDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur efficitur nec magna ac pharetra. Praesent sit amet est felis. Maecenas.";
        public const string courseName = "Test course 2021";

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

            resourceService = new ResourceService(context, mapper, fileService);

            lectureService = new LectureService(context, mapper,
                resourceService);

            courseService = new CourseService(context, mapper, lectureService);

            forumService = new ForumService(context, mapper);

            surveyService = new SurveyService(context, mapper);

            userService = new UserService(context, mapper, GetMockUserManager(context), GetMockRoleManager(context), courseService, surveyService, resourceService, fileService);
        }

        public string SeedCourse()
            => courseService.CreateCourse(new AddCourseFormModel
            {
                Name = courseName,
                Credits = 15,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                SignUpStartDate = DateTime.UtcNow.AddDays(-2),
                SignUpEndDate = DateTime.UtcNow.AddDays(10),
                Price = 220
            });

        public string SeedLecture(string courseId)
            => lectureService.AddLectureToCourse(courseId, new AddLectureFormModel
            {
                Name = "Very important test"
            });

        //Seeding non admin user
        public void SeedUser(string username = "test-username", string id = testUserId)
        {
            context.Users.Add(new ApplicationUser
            {
                Id = id,
                UserName = username,
                Email = testUserId
            });
            context.SaveChanges();
        }

        public string SeedResourceTypes(bool isRemote = true)
        {
            context.ResourceTypes.Add(new ResourceType() { Name = "Youtube link", Icon = "<i class=\"fab fa-youtube\"></i>", IsRemote = isRemote });

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

        public string SeedLectureWithDescriptions()
        {
            var courseId = SeedCourse();

            var lectureId = SeedLecture(courseId);

            //Act
            lectureService.EditLectureById(lectureId, new EditLectureFormModel()
            {
                CourseId = courseId,
                Descriptions = new List<EditDescriptionFormModel>()
                    { new EditDescriptionFormModel() { Name = "test" } }
                ,
                Name = "TEST"
            });

            return lectureId;
        }

        public void SeedUserExam(string courseId, string userId)
        {
            var exam = new Exam()
            {
                CourseId = courseId,
                Name = "Test-Exam",
                Password = "123456",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(5)
            };

            context.Exams.Add(exam);

            var userExam = new ApplicationUserExam()
            {
                ApplicationUserId = userId,
                EarnedCredits = 10,
                EarnedPoints = 10,
                ExamId = exam.Id,
            };

            context.ApplicationUserExams.Add(userExam);
            context.Users.First(x => x.Id == userId).Exams.Add(userExam);

            context.SaveChanges();
        }

        public void SeedUserCourseSurvey(string courseId, string userId)
        {
            var survey = new Survey()
            {
                CourseId = courseId,
                Name = "Test-Course",
                IsPublic = true,
            };
            context.Surveys.Add(survey);
            context.SaveChanges();
        }

        public void SeedUserCourse(string courseId, string userId)
        {
            context.UserCourses.Add(new UserCourse()
            {
                CourseId = courseId,
                UserId = userId,
                IsPaid = true,
            });
            context.SaveChanges();
        }

        public static RoleManager<IdentityRole> GetMockRoleManager(DeemZDbContext context)
        {
            var roleStore = new Mock<IRoleStore<IdentityRole>>();

            var roleManager = new Mock<RoleManager<IdentityRole>>(
                roleStore.Object, null, null, null, null);

            roleManager.Setup(x => x.RoleExistsAsync("Administrator")).ReturnsAsync(true);
            roleManager.Setup(x => x.RoleExistsAsync("Lecturer")).ReturnsAsync(true);
            roleManager.Setup(x => x.RoleExistsAsync("test")).ReturnsAsync(false);

            return roleManager.Object;
        }

        public static UserManager<ApplicationUser> GetMockUserManager(DeemZDbContext context)
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();

            var userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<ApplicationUser>(context.Users.FirstOrDefault(x => x.UserName == It.IsAny<string>())));
            userManager.Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), "Lecturer")).ReturnsAsync(true);
            userManager.Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), "test-role")).ReturnsAsync(false);

            return userManager.Object;
        }
    }
}