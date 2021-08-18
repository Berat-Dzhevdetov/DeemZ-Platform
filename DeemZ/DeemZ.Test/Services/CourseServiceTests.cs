namespace DeemZ.Test.Services
{
    using System;
    using System.Linq;
    using Xunit;
    using DeemZ.Models.FormModels.Course;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.Administration;

    public class CourseServiceTests : BaseTestClass
    {
        public CourseServiceTests() => SetUpServices();

        [Fact]
        public void AddCourseShouldAddCourseToTheDataBase()
        {
            //Arange
            var expectedCount = 1;

            //Act
            courseService.CreateCourse(new AddCourseFormModel
            {
                Name = "Test course 2021",
                Credits = 15,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                SignUpStartDate = DateTime.UtcNow.AddDays(-2),
                SignUpEndDate = DateTime.UtcNow.AddDays(-1),
                Price = 220
            });

            var actualCount = context.Courses.Count();

            //Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void CreateBasicLecturesShouldCreateBasicLectures()
        {
            //Arange
            var expectedCount = 2;

            var model = new AddCourseFormModel
            {
                Name = "Test course 2021",
                Credits = 15,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                SignUpStartDate = DateTime.UtcNow.AddDays(-2),
                SignUpEndDate = DateTime.UtcNow.AddDays(-1),
                Price = 220
            };

            var courseId = courseService.CreateCourse(model);

            //Act
            courseService.CreateBasicsLectures(courseId, model);

            var actualCount = context.Lectures.Count();

            //Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void DeleteCourseShouldDeleteCourseFromDb()
        {
            //Arange
            var expectedCount = 0;

            var courseId = SeedCourse();

            //Act
            courseService.DeleteCourse(courseId);

            var actualCount = context.Countries.Count();

            //Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void GetCourseByIdShouldReturnTrueIfCourseExists()
        {
            //Arange
            var courseId = SeedCourse();

            //Act
            var actualResult = courseService.GetCourseById(courseId);

            //Assert
            Assert.True(actualResult);
        }

        [Fact]
        public void GetCourseByIdShouldReturnFalseIfCourseDoesNotExists()
        {
            //Act
            var actualResult = courseService.GetCourseById("Invalid-Course-Id");

            //Assert
            Assert.False(actualResult);
        }

        [Fact]
        public void GetCourseByIdShouldReturnTheCourse()
        {
            //Arrange
            var courseId = SeedCourse();

            //Act
            var actualResult = courseService.GetCourseById<Course>(courseId).Name;

            //Assert
            Assert.Equal(courseName, actualResult);
        }

        [Fact]
        public void EditCourseShouldEditCourseDetails()
        {
            //Arrange
            var courseId = SeedCourse();

            //Act
            courseService.Edit(new EditCourseFormModel
            {
                Credits = 15,
                Name = "test"
            }, courseId);

            var actualResult = context.Courses.Find(courseId).Name;

            //Assert
            Assert.NotEqual(courseName, actualResult);
        }

        [Fact]
        public void SignUpForCourseShouldSignUpUserToCourse()
        {
            //Arrange
            var expectedUserCourseCount = 1;

            var courseId = SeedCourse();

            SeedUser();

            //Act
            courseService.SignUserToCourse(testUserId, courseId, true);

            var actualResult = context.UserCourses
                .Count(x => x.CourseId == courseId
                    && x.UserId == testUserId);

            //Assert
            Assert.Equal(expectedUserCourseCount, actualResult);
        }

        [Fact]
        public void AfterSignUpGetUserCourseCountShouldReturnCorrectNumer()
        {
            //Arrange
            var expectedUserCourseCount = 1;

            var courseId = SeedCourse();

            SeedUser();

            //Act
            courseService.SignUserToCourse(testUserId, courseId, true);

            var actualResult = courseService.GetUserCoursesCount();

            //Assert
            Assert.Equal(expectedUserCourseCount, actualResult);
        }

        [Fact]
        public void DeleteUserFromCourse()
        {
            //Arrange
            var expectedUserCourseCount = 0;

            var courseId = SeedCourse();

            SeedUser();

            //Act
            courseService.SignUserToCourse(testUserId, courseId, true);

            courseService.DeleteUserFromCourse(courseId, testUserId);

            var actualResult = context.UserCourses
                .Count(x => x.CourseId == courseId && x.UserId == testUserId);

            //Assert
            Assert.Equal(expectedUserCourseCount, actualResult);
        }

        [Fact]
        public void GettingUserCourses()
        {
            //Arrange
            var expectedUserCourseCount = 1;

            var courseId = SeedCourse();

            SeedUser();

            //Act
            courseService.SignUserToCourse(testUserId, courseId, true);

            var actualResult = courseService.GetUserCourses<UserCoursesViewModel>(1, 20)
                .Count();

            //Assert
            Assert.Equal(expectedUserCourseCount, actualResult);
        }
    }
}