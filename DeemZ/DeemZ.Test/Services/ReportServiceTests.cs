namespace DeemZ.Test.Services
{
    using AutoMapper;
    using DeemZ.Data;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Course;
    using DeemZ.Models.FormModels.Lecture;
    using DeemZ.Models.FormModels.Reports;
    using DeemZ.Models.ViewModels.Reports;
    using DeemZ.Services.AutoMapperProfiles;
    using DeemZ.Services.CourseServices;
    using DeemZ.Services.FileService;
    using DeemZ.Services.LectureServices;
    using DeemZ.Services.ReportService;
    using DeemZ.Services.ResourceService;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    //Course => Lecture =>ResourceService=>File Service
    public class ReportServiceTests
    {
        private readonly IReportService reportService;
        private readonly ILectureService lectureService;
        private readonly IResourceService resourceService;
        private readonly IFileService fileService;
        private readonly ICourseService courseService;

        private const string testUserId = "test-user";
        private const string issueDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur efficitur nec magna ac pharetra. Praesent sit amet est felis. Maecenas.";
        private readonly DeemZDbContext context;

        public object AutoMapperConfig { get; }

        public ReportServiceTests()
        {
            var options = new DbContextOptionsBuilder<DeemZDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            context = new DeemZDbContext(options.Options);
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ReportProfile());
                mc.AddProfile(new CourseProfile());
                mc.AddProfile(new LectureProfile());
                mc.AddProfile(new ResourceProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            reportService = new ReportService(context, mapper);

            fileService = new FileService(context);

            resourceService = new ResourceService(context, mapper, fileService);

            lectureService = new LectureService(context, mapper,
                resourceService);

            courseService = new CourseService(context, mapper, lectureService);
        }

        [Fact]
        public void AddReportFromServiceShouldAddReportToTheDataBase()
        {
            //Arange
            var expectedCount = 1;

            //Act
            reportService.AddReport(new AddReportFormModel { IssueDescription = issueDescription, LectureId = "test-lecture" }, testUserId);
            var reportsCount = context.Reports.ToArray().Length;

            //Assert
            Assert.Equal(expectedCount, reportsCount);
        }

        [Fact]
        public void GettingReportFromServiceShouldGetTheReports()
        {
            //Arange
            var expectedCount = 2;
            var couseId = courseService.CreateCourse(new AddCourseFormModel
            {
                Name = "Test course 2021",
                Credits = 15,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                SignUpStartDate = DateTime.UtcNow.AddDays(-2),
                SignUpEndDate = DateTime.UtcNow.AddDays(-1),
                Price = 220
            });

            var lectureId = lectureService.AddLectureToCourse(couseId, new AddLectureFormModel
            {
                Name = "Very important test"
            });

            context.Users.Add(new ApplicationUser
            {
                Id = testUserId,
                UserName = testUserId,
                Email = testUserId
            });



            //Act
            reportService.AddReport(new AddReportFormModel { IssueDescription = issueDescription, LectureId = lectureId }, testUserId);
            reportService.AddReport(new AddReportFormModel { IssueDescription = issueDescription, LectureId = lectureId }, testUserId);
            var countsInDb = reportService.GetReports<PreviewReportViewModel>(1, 20).ToArray().Length;
            ;
            //Assert
            Assert.Equal(expectedCount, countsInDb);
        }

        [Fact]
        public void DeletingReportShouldRemoveitFromTheDataBase()
        {
            //Arange
            var expectedCount = 0;
            var couseId = courseService.CreateCourse(new AddCourseFormModel
            {
                Name = "Test course 2021",
                Credits = 15,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                SignUpStartDate = DateTime.UtcNow.AddDays(-2),
                SignUpEndDate = DateTime.UtcNow.AddDays(-1),
                Price = 220
            });

            var lectureId = lectureService.AddLectureToCourse(couseId, new AddLectureFormModel
            {
                Name = "Very important test"
            });

            context.Users.Add(new ApplicationUser
            {
                Id = testUserId,
                UserName = testUserId,
                Email = testUserId
            }).State = EntityState.Detached;

            context.SaveChanges();

            //Act
            var reportId = reportService.AddReport(new AddReportFormModel { IssueDescription = issueDescription, LectureId = lectureId }, testUserId);
            reportService.Delete(reportId);

            var countsInDb = context.Reports.ToArray().Length;
            ;
            //Assert
            Assert.Equal(expectedCount, countsInDb);
        }
    }
}
