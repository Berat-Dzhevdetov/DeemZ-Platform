using System.Threading.Tasks;
using DeemZ.Models.FormModels.Exam;
using DeemZ.Services;
using DeemZ.Services.AdminServices;
using DeemZ.Services.ExamServices;
using DeemZ.Services.Question;
using DeemZ.Services.SurveyServices;
using Microsoft.AspNetCore.Identity;
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
    using DeemZ.Services.EmailSender;

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
        public IExamService examService;
        public IQuestionService questionService;
        public IAdminService adminService;
        public IEmailSenderService emailSender;
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

            fileService = new FileService(context);

            resourceService = new ResourceService(context, mapper, fileService);

            lectureService = new LectureService(context, mapper, resourceService);

            courseService = new CourseService(context, mapper, lectureService, null);

            reportService = new ReportService(context, mapper);
            forumService = new ForumService(context, mapper);
            surveyService = new SurveyService(context, mapper);
            examService = new ExamService(context, mapper);
            questionService = new QuestionService(context, mapper);
            
            emailSender = new EmailSenderService(context);

            userService = new UserService(context, mapper, GetMockUserManager(context), GetMockRoleManager(context), courseService, surveyService, resourceService, fileService, emailSender);

            adminService = new AdminService(mapper, context, courseService, userService);
        }

        public async Task<string> SeedCourse()
            => await courseService.CreateCourse(new AddCourseFormModel
            {
                Name = courseName,
                Credits = 15,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                SignUpStartDate = DateTime.UtcNow.AddDays(-2),
                SignUpEndDate = DateTime.UtcNow.AddDays(10),
                Price = 220
            });

        public async Task<string> SeedLecture(string courseId)
            => await lectureService.AddLectureToCourse(courseId, new AddLectureFormModel
            {
                Name = "Very important test"
            });

        public async Task<string> SeedSurvey(string courseId)
        {
            var survey = new Survey
            {
                CourseId = courseId,
                StartDate = DateTime.UtcNow.AddDays(-10),
                EndDate = DateTime.UtcNow.AddDays(20),
                Name = "test-survey"
            };

            context.Surveys.Add(survey);
            await context.SaveChangesAsync();

            return survey.Id;
        }

        //Seeding non admin user
        public async Task SeedUser(string username = "test-username", string id = testUserId)
        {
            context.Users.Add(new ApplicationUser
            {
                Id = id,
                UserName = username,
                Email = testUserId
            });
            await context.SaveChangesAsync();
        }

        public async Task<string> SeedResourceTypes(bool isRemote = true)
        {
            context.ResourceTypes.Add(new ResourceType() { Name = "Youtube link", Icon = "<i class=\"fab fa-youtube\"></i>", IsRemote = isRemote });

            await context.SaveChangesAsync();

            return context.ResourceTypes.First().Id;
        }

        public async Task<string> SeedResource()
        {
            var courseId = await SeedCourse();

            var lectureId = await SeedLecture(courseId);

            var resourceTypeId = await SeedResourceTypes();

            var resourceToAdd = new AddResourceFormModel
            {
                Name = "Test",
                Path = "Test-path",
                ResourceTypeId = resourceTypeId
            };

            return await resourceService.AddResourceToLecture(lectureId, testUserId, resourceToAdd);
        }

        public async Task<string> SeedForumTopic(bool addUser = true)
        {
            if (addUser) await SeedUser();
            return await forumService.CreateTopic(new CreateForumTopicFormModel()
            {
                Title = "Test",
                Description = "Test",
            }, testUserId);
        }

        public async Task<string> SeedLectureWithDescriptions()
        {
            var courseId = await SeedCourse();

            var lectureId = await SeedLecture(courseId);

            //Act
            await lectureService.EditLectureById(lectureId, new EditLectureFormModel()
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

        public string SeedCourseSurvey(string courseId)
        {
            var survey = new Survey()
            {
                CourseId = courseId,
                Name = "Test-Course",
                EndDate = DateTime.Today.AddDays(999),
            };

            context.Surveys.Add(survey);
            context.SaveChanges();

            return survey.Id;
        }

        public void SeedUserSurvey(string userId, string surveyId)
        {
            context.ApplicationUserSurveys.Add(new ApplicationUserSurvey()
            {
                ApplicationUserId = userId,
                SurveyId = surveyId,
            });
            context.SaveChanges();
        }

        public void SeedUserCourseSurvey(string courseId, string userId)
        {
            courseService.SignUserToCourse(userId, courseId);
            var survey = new Survey()
            {
                CourseId = courseId,
                Name = "Test-Course",
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
                Paid = 220,
                PaidOn = DateTime.Today,
            });

            context.SaveChanges();
        }

        public string SeedExam(string courseId)
        {
            examService.CreateExam(courseId, new AddExamFormModel()
            {
                Name = "TestExam",
                StartDate = DateTime.Today.AddDays(-1),
                EndDate = DateTime.Today.AddDays(15),
                IsPublic = true,
                Password = "HackMe"
            });

            return context.Exams.First().Id;
        }

        public void SeedExamQuestions(string examId)
        {
            context.Exams.First(x => x.Id == examId).Questions.Add(
                new Question()
                {
                    Text = "Are you cool?",
                    Points = 99,
                    ExamId = examId,
                });
            context.SaveChanges();
        }

        public void SeedExamQuestionsAnswers(string examId)
        {
            SeedExamQuestions(examId);
            context.Answers.Add(new Answer()
            {
                Text = "You are cool",
                IsCorrect = true,
                QuestionId = context.Questions.First().Id
            });
            context.SaveChanges();
        }

        public List<TakeExamQuestionFormModel> GetTakeExamQuestionFormModels(string examId)
        {
            context.Questions.Add(new Question()
            {
                ExamId = examId,
                Points = 10,
                Text = "Test-question",
                Id = "test-question-id"
            });

            context.Answers.Add(new Answer()
            {
                Id = "test-answer-id",
                IsCorrect = true,
                Text = "test",
                QuestionId = "test-question-id",
            });

            context.SaveChanges();

            var questions = new List<TakeExamQuestionFormModel>()
            {
                new TakeExamQuestionFormModel()
                {
                    Id = "test-question-id",
                    Points = 10,
                    Text = "test-question",
                    Answers = new List<TakeExamQuestionAnswerFormModel>()
                    {
                        new TakeExamQuestionAnswerFormModel(){IsChosen = true,Text = "Test",Id = "test-answer-id"}
                    },
                }
            };

            return questions;
        }

        public string SeedExpiredExam(string courseId)
        {
            examService.CreateExam(courseId, new AddExamFormModel()
            {
                Name = "TestExam",
                StartDate = DateTime.Today.AddDays(-20),
                EndDate = DateTime.Today.AddDays(-10),
                IsPublic = true,
                Password = "HackMe"
            });

            return context.Exams.First().Id;
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