namespace DeemZ.Test.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using DeemZ.Models.ViewModels.Surveys;
    using Xunit;
    public class SurveyServiceTests : BaseTestClass
    {
        public SurveyServiceTests()
        {
            SetUpServices();
        }

        [Fact]
        public async Task GettingUserSurveysShouldReturnTheCorrectSurveys()
        {
            var expectedUserSurveysCount = 1;

            var courseId = await SeedCourse();

            var userId = "test-user";
            await SeedUser();

            SeedUserExam(courseId, userId);
            SeedUserCourseSurvey(courseId, userId);

            var actualUserSurveysCount = (await surveyService.GetUserCurrentCourseSurveys<IndexSurveyViewModel>(userId, false)).Count();

            Assert.Equal(expectedUserSurveysCount, actualUserSurveysCount);
        }

        [Fact]
        public async Task GettingSurveyByIdShouldReturnTheCorrectSurvey()
        {
            var courseId = await SeedCourse();

            SeedCourseSurvey(courseId);
            
            var expectedSurveyId = context.Surveys.First().Id;

            var actualSurveyId = (await surveyService.GetSurveyById<IndexSurveyViewModel>(expectedSurveyId)).Id;

            Assert.Equal(expectedSurveyId,actualSurveyId);
        }

        [Fact]
        public async Task DoesUserHavePermissionShouldReturnFalseWhenUserIsNotEnrolled()
        {
            var courseId = await SeedCourse();

            var userId = "test-user";
            await SeedUser();

            SeedCourseSurvey(courseId);
            var surveyId = context.Surveys.First().Id;

            var result = surveyService.DoesTheUserHavePermissionToSurvey(userId, surveyId);

            Assert.False(result);
        }

        [Fact]
        public async Task DoesUserHavePermissionShouldReturnTrueWhenUserIsEnrolledAndHasNotTakenTheSurvey()
        {
            var courseId = await SeedCourse();

            var userId = "test-user";
            await SeedUser();

            SeedCourseSurvey(courseId);
            SeedUserCourseSurvey(courseId,userId);
            var surveyId = context.Surveys.First().Id;

            var result = surveyService.DoesTheUserHavePermissionToSurvey(userId, surveyId);

            Assert.True(result);
        }

        [Fact]
        public async Task DoesUserHavePermissionShouldReturnFalseWhenUserIsEnrolledAndHasTakenTheSurvey()
        {
            var courseId = await SeedCourse();

            var userId = "test-user";
            await SeedUser();

            SeedUserCourseSurvey(courseId, userId);

            var surveyId = context.Surveys.First().Id;
            SeedUserSurvey(userId, surveyId);

            var result = surveyService.DoesTheUserHavePermissionToSurvey(userId, surveyId);

            Assert.False(result);
        }
    }
}
