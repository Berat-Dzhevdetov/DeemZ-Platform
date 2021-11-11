using System.Linq;
using System.Threading.Tasks;
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
        public async Task GettingUserCoursesShouldReturnTheCorrectAmountOfCourses()
        {
            var expectedUserCourses = 1;

            var courseId = await SeedCourse();
            
            await SeedUser();
            var userId = "test-user";
            
             SeedUserCourse(courseId, userId);

            var actualUserCourses = adminService.GetUserCourses<UserCoursesViewModel>().Count();

            Assert.Equal(expectedUserCourses,actualUserCourses);
        }

        [Fact]
        public async Task GettingUserCoursesCountShouldReturnTheCorrectAmountOfCourses()
        {
            var expectedUserCourses = 1;

            var courseId = await SeedCourse();

            await SeedUser();
            var userId = "test-user";

            SeedUserCourse(courseId, userId);

            var actualUserCourses = adminService.GetUserCoursesCount();

            Assert.Equal(expectedUserCourses, actualUserCourses);
        }

        [Fact]
        public async Task GettingAllCoursesShouldReturnTheCorrectAmountOfCourses()
        {
            var expectedCoursesCount = 1;
            var expectedCourseName = courseName;

            await SeedCourse();

            var actualCoursesCount = adminService.GetAllCourses<CoursesViewModel>().Count();
            var actualCourseName = adminService.GetAllCourses<CoursesViewModel>().First().Name;

            Assert.Equal(expectedCourseName,actualCourseName);
            Assert.Equal(expectedCoursesCount,actualCoursesCount);
        }

        [Fact]
        public async Task GetIndexPageInfoShouldReturnCorrectInfo()
        {
            var expectedUsers = 1;
            var expectedCourses = 1;
            var expectedMoney = 220;
            var expectedUsersSignedUpThisMonth = 1;

            var courseId = await SeedCourse();

            await SeedUser();
            var userId = "test-user";

            SeedUserCourse(courseId, userId);

            var indexInfo = adminService.GetIndexPageInfo();

            Assert.Equal(expectedCourses,indexInfo.TotalCourses);
            Assert.Equal(expectedUsers,indexInfo.UsersCount);
            Assert.Equal(expectedMoney, indexInfo.MoneyEarnedThisMonth); //not ok
            Assert.Equal(expectedUsersSignedUpThisMonth, indexInfo.UsersSignedUpThisMonth);
        }

        [Fact]
        public async Task GetUserSignUpForCourseShouldReturnTheCorrectAmount()
        {
            var expectedCount = 1;

            var courseId = await SeedCourse();

            await SeedUser();
            var userId = "test-user";

            SeedUserCourse(courseId, userId);

            var actualCount = adminService.GetUserSignUpForCourse(courseId);

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task GetUserSignedUpForCourseShouldReturnTheCorrectAmount()
        {
            var expectedCount = 1;

            var courseId =await SeedCourse();

            await SeedUser();
            var userId = "test-user";

            SeedUserCourse(courseId, userId);

            var actualCount = adminService.GetUserSignedUpForCourse(courseId);

            Assert.Equal(expectedCount, actualCount);
        }
    }
}
