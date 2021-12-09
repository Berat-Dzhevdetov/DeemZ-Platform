namespace DeemZ.Test.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using DeemZ.Data.Models;
    using DeemZ.Global.WebConstants;
    using DeemZ.Models.FormModels.User;
    using DeemZ.Models.ViewModels.User;
    using Xunit;
    public class UserServiceTests : BaseTestClass
    {
        public UserServiceTests()
        {
            SetUpServices();
        }

        [Fact]
        public async Task EditingUserShouldChangeTheirProperties()
        {
            var expectedUsername = "TEST";
            await SeedUser();

            var userId = context.Users.First().Id;

            await userService.EditUser(userId, new EditUserFormModel()
            {
                UserName = testUserId,
                Email = testUserId,
                FirstName = expectedUsername,
            });

            var actualUsername = context.Users.First().FirstName;

            Assert.Equal(expectedUsername, actualUsername);
        }

        [Fact]
        public async Task GettingAllUsersShouldReturnTheCorrectUsers()
        {
            var expectedFirstUserUsername = "Bobi";
            await SeedUser("Bobi", "user-1");
            await SeedUser("Vlado", "user-2");
            await SeedUser("Bero", "user-3");

            var actualFirstUserUsername = userService.GetAllUsers<BasicUserInformationViewModel>().First().Username;

            Assert.Equal(expectedFirstUserUsername, actualFirstUserUsername);
        }

        [Fact]
        public async Task GettingUserByIdShouldReturnTheCorrectUser()
        {
            var expectedUsername = "test-username";
            await SeedUser(expectedUsername);

            var userId = context.Users.First().Id;

            var actualUsername = (await userService.GetUserById<BasicUserInformationViewModel>(userId)).Username;

            Assert.Equal(expectedUsername, actualUsername);
        }

        [Fact]
        public async Task GettingUserByIdShouldReturnTrueIfUserExists()
        {
            await SeedUser();

            var userId = context.Users.First().Id;

            var doesUserExist = userService.GetUserById(userId);

            Assert.True(doesUserExist);
        }

        [Fact]
        public void GettingUserByIdShouldReturnFalseIfUserDoesNotExists() =>
            Assert.False(userService.GetUserById("invalid-id"));

        [Fact]
        public async Task IsEmailFreeShouldReturnTrueIfEmailIsFree()
        {
            await SeedUser();
            var userId = context.Users.First().Id;

            var isEmailFree = userService.IsEmailFree(userId, "test@test.com");
            Assert.True(isEmailFree);
        }

        [Fact]
        public async Task IsEmailFreeShouldReturnTrueIfEmailIsTaken()
        {
            await SeedUser();
            var userId = context.Users.First().Id;

            var isEmailFree = userService.IsEmailFree(userId, "test-user");
            Assert.True(isEmailFree);
        }

        [Fact]
        public async Task IsUsernameFreeShouldReturnTrueIfUsernameIsFree()
        {
            await SeedUser();
            var userId = context.Users.First().Id;

            var isUsernameFree = userService.IsUsernameFree(userId, "test-username-123");
            Assert.True(isUsernameFree);
        }

        [Fact]
        public async Task IsUsernameFreeShouldReturnTrueIfUsernameIsTaken()
        {
            await SeedUser();
            var userId = context.Users.First().Id;

            var isUsernameFree = userService.IsUsernameFree(userId, "test-username");
            Assert.True(isUsernameFree);
        }

        [Fact]
        public async Task GetUserByUsernameShouldReturnTrueOnCorrectUsername()
        {
            var expectedUsername = "test-username";

            await SeedUser(expectedUsername);

            Assert.True(userService.GetUserByUserName(expectedUsername));
        }

        [Fact]
        public void GetUserByUsernameShouldReturnFalseOnInCorrectUsername() =>
            Assert.False(userService.GetUserByUserName("invalid-username"));

        [Fact]
        public async Task GetUserByUserNameShouldReturnCorrectUserId()
        {
            var expectedUserId = "testing";
            var username = "Bobby";
            await SeedUser(username, expectedUserId);

            var actualUserId = await userService.GetUserIdByUserName(username);

            Assert.Equal(expectedUserId, actualUserId);
        }

        [Fact]
        public async Task AddingUserToRoleShouldAddThemSuccessfully()
        {
            await SeedUser();
            var userId = "test-user";
            await userService.AddUserToRole(userId, Constant.Role.LecturerRoleName);

            var isAdded = await userService.IsInRoleAsync(userId, Constant.Role.LecturerRoleName);

            Assert.True(isAdded);
        }

        [Fact]
        public async Task AddingUserToRoleWillNotAddThemIfRoleDoesNotExist()
        {
            await SeedUser();
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
        public async Task SetProfileImgShouldSetTheCorrectUrl()
        {
            var userId = "test-user";
            var photoPublicId = "public-photo-id";
            var photoUrl = "https://api.images.com/cat";

            await SeedUser();

            await userService.SetProfileImg(userId,photoUrl,photoPublicId);

            var user = await userService.GetUserById<ApplicationUser>(userId);

            Assert.Equal(photoPublicId, user.ImgPublicId);
            Assert.Equal(photoUrl,user.ImgUrl);
        }

        [Fact]
        public async Task DeletingUserImgShouldRemoveItFromCloudinary()
        {
            var userId = "test-user";
            var photoPublicId = "public-photo-id";
            var photoUrl = "https://api.images.com/cat";

            await SeedUser();

            await userService.SetProfileImg(userId, photoUrl, photoPublicId);

            await userService.DeleteUserProfileImg(userId);

            //Can't really test cloudinary functionality
        }

        [Fact]
        public async Task DeletingUserImgShouldNotDoAnythingIfItIsTheDefaultImg()
        {
            var userId = "test-user";
            await SeedUser();

            await userService.DeleteUserProfileImg(userId);

            //Can't really test cloudinary functionality
        }

        [Fact]
        public async Task GettingUserCoursesShouldReturnTheCorrectCoursesCount()
        {
            var expectedCoursesCount = 1;

            var courseId = await SeedCourse();
            var userId = "test-user";
            await SeedUser();

            await courseService.SignUserToCourse(userId,courseId);

            var userCourses = await userService.GetUserTakenCourses(userId);

            Assert.Equal(expectedCoursesCount,userCourses);
        }

        [Fact]
        public async Task GettingUserIndexInformationShouldGetAllUserInfo()
        {
            var courseId = await SeedCourse();
            var creditsCount = 10;
            var userSurveysCount = 1;
            var userId = "test-user";
            await SeedUser();
            
            await courseService.SignUserToCourse(userId, courseId);

            SeedUserExam(courseId,userId);
            var surveyId = SeedCourseSurvey(courseId);
            SeedUserSurvey(userId, surveyId);
            var userInfo = await userService.GetIndexInformaiton(userId,true);
            
            Assert.Equal(courseId, userInfo.Courses.First().Id);
            Assert.Equal(creditsCount, userInfo.Credits);
            Assert.Equal(userSurveysCount, userInfo.Surveys.Count());

        }
    }
}
