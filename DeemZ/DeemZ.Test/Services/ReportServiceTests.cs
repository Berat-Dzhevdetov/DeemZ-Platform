namespace DeemZ.Test.Services
{
    using System.Linq;
    using Xunit;
    using DeemZ.Models.FormModels.Reports;
    using DeemZ.Models.ViewModels.Reports;
    using System.Threading.Tasks;

    public class ReportServiceTests : BaseTestClass
    {
        public ReportServiceTests() => SetUpServices();

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
        public async Task GettingReportFromServiceShouldGetTheReports()
        {
            //Arange
            var expectedCount = 2;
            var courseId = await SeedCourse();

            var lectureId = SeedLecture(courseId);

            SeedUser();

            //Act
            reportService.AddReport(new AddReportFormModel { IssueDescription = issueDescription, LectureId = lectureId }, testUserId);
            reportService.AddReport(new AddReportFormModel { IssueDescription = issueDescription, LectureId = lectureId }, testUserId);
            var countsInDb = reportService.GetReports<PreviewReportViewModel>(1, 20).ToArray().Length;
            ;
            //Assert
            Assert.Equal(expectedCount, countsInDb);
        }

        [Fact]
        public async Task DeletingReportShouldRemoveTtFromTheDataBase()
        {
            //Arange
            var expectedCount = 0;
            var courseId =await SeedCourse();

            var lectureId = SeedLecture(courseId);

            SeedUser();

            //Act
            var reportId = reportService.AddReport(new AddReportFormModel { IssueDescription = issueDescription, LectureId = lectureId }, testUserId);
            reportService.Delete(reportId);

            var countsInDb = context.Reports.ToArray().Length;

            //Assert
            Assert.Equal(expectedCount, countsInDb);
        }

        [Fact]
        public async Task GettingReportByIdFromServiceShouldGetTheReport()
        {
            //Arange
            var courseId = await SeedCourse();

            var lectureId = SeedLecture(courseId);

            SeedUser();

            //Act
            var reportId = reportService.AddReport(new AddReportFormModel { IssueDescription = issueDescription, LectureId = lectureId }, testUserId);
            var report = reportService.GetReportById<PreviewReportViewModel>(reportId);

            //Assert
            Assert.NotNull(report);
        }

        [Fact]
        public async Task GettingReportByIdFromServiceShouldReturnTrueIfReportExists()
        {
            //Arange
            var courseId = await SeedCourse();

            var lectureId = SeedLecture(courseId);

            SeedUser();

            //Act
            var reportId = reportService.AddReport(new AddReportFormModel { IssueDescription = issueDescription, LectureId = lectureId }, testUserId);
            var report = reportService.GetReportById(reportId);

            //Assert
            Assert.True(report);
        }
        [Fact]
        public void GettingReportByIdFromServiceShouldReturnFalseIfReportDoesNotExists() => Assert.False(reportService.GetReportById("invalid-id"));
    }
}