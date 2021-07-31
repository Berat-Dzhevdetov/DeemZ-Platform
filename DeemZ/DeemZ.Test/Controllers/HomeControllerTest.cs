namespace DeemZ.Test
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using DeemZ.App.Controllers;
    using DeemZ.Models.ViewModels.User;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnNotAuthorizedViewIfThereIsNoUser()
            => MyMvc
            .Controller<HomeController>()
                .Calling(c => c.Index())
            .ShouldReturn()
            .Ok();

        [Fact]
        public void IndexShouldReturnAuthorizedViewIfThereIsUser()
            => MyMvc
            .Controller<HomeController>(instance => instance
                .WithUser("TestingUser"))
            .Calling(c => c.Index())
            .ShouldReturn()
            .Ok(result => result
                .WithModelOfType<IndexUserViewModel>());
    }
}