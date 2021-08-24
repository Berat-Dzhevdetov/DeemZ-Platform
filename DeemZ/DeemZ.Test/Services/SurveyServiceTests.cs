using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeemZ.Models.ViewModels.Surveys;
using Xunit;

namespace DeemZ.Test.Services
{
    public class SurveyServiceTests : BaseTestClass
    {
        public SurveyServiceTests()
        {
            SetUpServices();
        }

        [Fact]
        public void GettingUserSurveysShouldReturnTheCorrectSurveys()
        {
            var expectedUserSurveysCount = 1;

            var courseId = SeedCourse();

            var userId = "test-user";
            SeedUser();

            SeedUserExam(courseId, userId);
            SeedUserCourseSurvey(courseId, userId);

            var actualUserSurveysCount = surveyService.GetUserCurrentCourseSurveys<IndexSurveyViewModel>(userId).Count();

            Assert.Equal(expectedUserSurveysCount, actualUserSurveysCount);
        }

        [Fact]
        public void GettingSurveyByIdShouldReturnTheCorrectSurvey()
        {
            var courseId = SeedCourse();

            SeedCourseSurvey(courseId);
            
            var expectedSurveyId = context.Surveys.First().Id;

            var actualSurveyId = surveyService.GetSurveyById<IndexSurveyViewModel>(expectedSurveyId).Id;

            Assert.Equal(expectedSurveyId,actualSurveyId);
        }

        [Fact]
        public void DoesUserHavePermissionShouldReturnFalseWhenUserIsNotEnrolled()
        {
            var courseId = SeedCourse();

            var userId = "test-user";
            SeedUser();

            SeedCourseSurvey(courseId);
            var surveyId = context.Surveys.First().Id;

            var result = surveyService.DoesTheUserHavePermissionToSurvey(userId, surveyId);

            Assert.False(result);
        }

        [Fact]
        public void DoesUserHavePermissionShouldReturnTrueWhenUserIsEnrolledAndHasNotTakenTheSurvey()
        {
            var courseId = SeedCourse();

            var userId = "test-user";
            SeedUser();

            SeedCourseSurvey(courseId);
            SeedUserCourseSurvey(courseId,userId);
            var surveyId = context.Surveys.First().Id;

            var result = surveyService.DoesTheUserHavePermissionToSurvey(userId, surveyId);

            Assert.True(result);
        }

        [Fact]
        public void DoesUserHavePermissionShouldReturnFalseWhenUserIsEnrolledAndHasTakenTheSurvey()
        {
            var courseId = SeedCourse();

            var userId = "test-user";
            SeedUser();

            SeedUserCourseSurvey(courseId, userId);

            var surveyId = context.Surveys.First().Id;
            SeedUserSurvey(userId, surveyId);

            var result = surveyService.DoesTheUserHavePermissionToSurvey(userId, surveyId);

            Assert.False(result);
        }
    }
}
