using System.Linq;
using System.Threading.Tasks;
using DeemZ.Data.Models;
using DeemZ.Global.WebConstants;
using DeemZ.Models.FormModels.User;
using DeemZ.Models.ViewModels.User;
using Xunit;

namespace DeemZ.Test.Services
{
    public class UserServiceTests : BaseTestClass
    {
        public UserServiceTests()
        {
            SetUpServices();
        }

        [Fact]
        public void EditingUserShouldChangeTheirProperties()
        {
            var expectedUsername = "TEST";
            SeedUser();

            var userId = context.Users.First().Id;

            userService.EditUser(userId, new EditUserFormModel()
            {
                UserName = testUserId,
                Email = testUserId,
                FirstName = expectedUsername,
            });

            var actualUsername = context.Users.First().FirstName;

            Assert.Equal(expectedUsername, actualUsername);
        }

        [Fact]
        public void GettingAllUsersShouldReturnTheCorrectUsers()
        {
            var expectedFirstUserUsername = "Bobi";
            SeedUser("Bobi", "user-1");
            SeedUser("Vlado", "user-2");
            SeedUser("Bero", "user-3");

            var actualFirstUserUsername = userService.GetAllUsers<BasicUserInformationViewModel>().First().Username;

            Assert.Equal(expectedFirstUserUsername, actualFirstUserUsername);
        }

        [Fact]
        public void GettingUserByIdShouldReturnTheCorrectUser()
        {
            var expectedUsername = "test-username";
            SeedUser(expectedUsername);

            var userId = context.Users.First().Id;

            var actualUsername = userService.GetUserById<BasicUserInformationViewModel>(userId).Username;

            Assert.Equal(expectedUsername, actualUsername);
        }

        [Fact]
        public void GettingUserByIdShouldReturnTrueIfUserExists()
        {
            SeedUser();

            var userId = context.Users.First().Id;

            var doesUserExist = userService.GetUserById(userId);

            Assert.True(doesUserExist);
        }

        [Fact]
        public void GettingUserByIdShouldReturnFalseIfUserDoesNotExists() =>
            Assert.False(userService.GetUserById("invalid-id"));

        [Fact]
        public void IsEmailFreeShouldReturnTrueIfEmailIsFree()
        {
            SeedUser();
            var userId = context.Users.First().Id;

            var isEmailFree = userService.IsEmailFree(userId, "test@test.com");
            Assert.True(isEmailFree);
        }

        [Fact]
        public void IsEmailFreeShouldReturnFalseIfEmailIsTaken()
        {
            SeedUser();
            var userId = context.Users.First().Id;

            var isEmailFree = userService.IsEmailFree(userId, "test-user");
            Assert.False(isEmailFree);
        }

        [Fact]
        public void IsUsernameFreeShouldReturnTrueIfUsernameIsFree()
        {
            SeedUser();
            var userId = context.Users.First().Id;

            var isUsernameFree = userService.IsUsernameFree(userId, "test-username-123");
            Assert.True(isUsernameFree);
        }

        [Fact]
        public void IsUsernameFreeShouldReturnFalseIfUsernameIsTaken()
        {
            SeedUser();
            var userId = context.Users.First().Id;

            var isUsernameFree = userService.IsUsernameFree(userId, "test-username");
            Assert.False(isUsernameFree);
        }

        [Fact]
        public void GetUserByUsernameShouldReturnTrueOnCorrectUsername()
        {
            var expectedUsername = "test-username";

            SeedUser(expectedUsername);

            Assert.True(userService.GetUserByUserName(expectedUsername));
        }

        [Fact]
        public void GetUserByUsernameShouldReturnFalseOnInCorrectUsername() =>
            Assert.False(userService.GetUserByUserName("invalid-username"));

        [Fact]
        public void GetUserByUserNameShouldReturnCorrectUserId()
        {
            var expectedUserId = "testing";
            var username = "Bobby";
            SeedUser(username, expectedUserId);

            var actualUserId = userService.GetUserIdByUserName(username);

            Assert.Equal(expectedUserId, actualUserId);
        }

        [Fact]
        public async Task AddingUserToRoleShouldAddThemSuccessfully()
        {
            SeedUser();
            var userId = "test-user";
            await userService.AddUserToRole(userId, Constant.Role.LecturerRoleName);

            var isAdded = await userService.IsInRoleAsync(userId, Constant.Role.LecturerRoleName);

            Assert.True(isAdded);
        }

        [Fact]
        public async Task AddingUserToRoleWillNotAddThemIfRoleDoesNotExist()
        {
            SeedUser();
            var userId = "test-user";
            var roleName = "test-role";
            await userService.AddUserToRole(userId, roleName);

            var isAdded = await userService.IsInRoleAsync(userId, roleName);

            Assert.False(isAdded);
        }

        [Fact]
        public async Task IsUserInRoleAsyncShouldSayThatUserIsNotInRoleIfHeIsNot() =>
            Assert.False(await userService.IsInRoleAsync("invalid-user-id", "invalid-role"));

        [Fact]
        public void SetProfileImgShouldSetTheCorrectUrl()
        {
            var userId = "test-user";
            var photoPublicId = "public-photo-id";
            var photoUrl = "https://api.images.com/cat";
            
            SeedUser();

            userService.SetProfileImg(userId,photoUrl,photoPublicId);

            var user = userService.GetUserById<ApplicationUser>(userId);

            Assert.Equal(photoPublicId, user.ImgPublicId);
            Assert.Equal(photoUrl,user.ImgUrl);
        }

        [Fact]
        public void DeletingUserImgShouldRemoveItFromCloudinary()
        {
            var userId = "test-user";
            var photoPublicId = "public-photo-id";
            var photoUrl = "https://api.images.com/cat";

            SeedUser();

            userService.SetProfileImg(userId, photoUrl, photoPublicId);

            userService.DeleteUserProfileImg(userId);

            //Can't really test cloudinary functionality
        }

        [Fact]
        public void DeletingUserImgShouldNotDoAnythingIfItIsTheDefaultImg()
        {
            var userId = "test-user";
            SeedUser();

            userService.DeleteUserProfileImg(userId);

            //Can't really test cloudinary functionality
        }

        [Fact]
        public void GettingUserCoursesShouldReturnTheCorrectCoursesCount()
        {
            var expectedCoursesCount = 1;

            var courseId = SeedCourse();
            var userId = "test-user";
            SeedUser();

            courseService.SignUserToCourse(userId,courseId);

            var userCourses = userService.GetUserTakenCourses(userId);

            Assert.Equal(expectedCoursesCount,userCourses);
        }

        [Fact]
        public void GettingUserIndexInformationShouldGetAllUserInfo()
        {
            var courseId = SeedCourse();
            var creditsCount = 10;
            var userSurveysCount = 1;
            var userId = "test-user";
            SeedUser();
            
            courseService.SignUserToCourse(userId, courseId);

            SeedUserExam(courseId,userId);
            SeedCourseSurvey(courseId);
            var userInfo = userService.GetIndexInformaiton(userId,true);
            
            Assert.Equal(courseId, userInfo.Courses.First().Id);
            Assert.Equal(creditsCount, userInfo.Credits);
            Assert.Equal(userSurveysCount, userInfo.Surveys.Count());

        }
    }
}
