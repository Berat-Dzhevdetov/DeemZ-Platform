using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Assert.Equal(expectedUsername,actualUsername);
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
            SeedUser(username,expectedUserId);

            var actualUserId = userService.GetUserIdByUserName(username);

            Assert.Equal(expectedUserId,actualUserId);
        }
    }
}
