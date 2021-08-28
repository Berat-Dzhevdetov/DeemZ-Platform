using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeemZ.Global.WebConstants;
using DeemZ.Models.ViewModels.Administration;
using Xunit;

namespace DeemZ.Test.Services
{
    public class AdminServiceTests : BaseTestClass
    {
        public AdminServiceTests()
        {
            SetUpServices();
        }

        [Fact]
        public void GettingUserCoursesShouldReturnTheCorrectAmountOfCourses()
        {
            var expectedUserCourses = 1;

            var courseId = SeedCourse();
            
            SeedUser();
            var userId = "test-user";
            
            SeedUserCourse(courseId, userId);

            var actualUserCourses = adminService.GetUserCourses<UserCoursesViewModel>().Count();

            Assert.Equal(expectedUserCourses,actualUserCourses);
        }

        [Fact]
        public void GettingUserCoursesCountShouldReturnTheCorrectAmountOfCourses()
        {
            var expectedUserCourses = 1;

            var courseId = SeedCourse();

            SeedUser();
            var userId = "test-user";

            SeedUserCourse(courseId, userId);

            var actualUserCourses = adminService.GetUserCoursesCount();

            Assert.Equal(expectedUserCourses, actualUserCourses);
        }

        [Fact]
        public void GettingAllCoursesShouldReturnTheCorrectAmountOfCourses()
        {
            var expectedCoursesCount = 1;
            var expectedCourseName = courseName;

            SeedCourse();

            var actualCoursesCount = adminService.GetAllCourses<CoursesViewModel>().Count();
            var actualCourseName = adminService.GetAllCourses<CoursesViewModel>().First().Name;

            Assert.Equal(expectedCourseName,actualCourseName);
            Assert.Equal(expectedCoursesCount,actualCoursesCount);
        }

        [Fact]
        public void GetIndexPageInfoShouldReturnCorrectInfo()
        {
            var expectedUsers = 1;
            var expectedCourses = 1;
            var expectedMoney = 220;
            var expectedUsersSignedUpThisMonth = 1;

            var courseId = SeedCourse();

            SeedUser();
            var userId = "test-user";

            SeedUserCourse(courseId, userId);

            var indexInfo = adminService.GetIndexPageInfo();
            ;

            Assert.Equal(expectedCourses,indexInfo.TotalCourses);
            Assert.Equal(expectedUsers,indexInfo.UsersCount);
            Assert.Equal(expectedMoney, indexInfo.MoneyEarnedThisMonth);
            Assert.Equal(expectedUsersSignedUpThisMonth, indexInfo.UsersSignedUpThisMonth);
        }

        [Fact]
        public void GetUserSignUpForCourseShouldReturnTheCorrectAmount()
        {
            var expectedCount = 1;

            var courseId = SeedCourse();

            SeedUser();
            var userId = "test-user";

            SeedUserCourse(courseId, userId);

            var actualCount = adminService.GetUserSignUpForCourse(courseId);

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void GetUserSignedUpForCourseShouldReturnTheCorrectAmount()
        {
            var expectedCount = 1;

            var courseId = SeedCourse();

            SeedUser();
            var userId = "test-user";

            SeedUserCourse(courseId, userId);

            var actualCount = adminService.GetUserSignedUpForCourse(courseId);

            Assert.Equal(expectedCount, actualCount);
        }
    }
}
