namespace DeemZ.Test.PipeLines
{
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using Xunit;
    using DeemZ.Models.ViewModels.Course;
    using DeemZ.Models.ViewModels.InformativeMessages;
    using DeemZ.Models.ViewModels.User;
    using DeemZ.Web.Controllers;

    using static DeemZ.Global.WebConstants.Constant;
    using DeemZ.Data.Models;

    public class HomeControllerTests
    {
        [Fact]
        public void HomeIndexShouldReturnTheViewForNotLoggedUserIfThereIsNotALoggedOne()
            => MyMvc
            .Controller<HomeController>()
            .Calling(x => x.Index())
            .ShouldReturn()
            .View(x => x.WithNoModel());

        [Fact]
        public void HomeIndexShouldReturnTheViewForLoggedUserIfThereIsALoggedOne()
            => MyMvc
            .Controller<HomeController>(instance => instance
                .WithUser())
            .Calling(x => x.Index())
            .ShouldHave()
            .MemoryCache(cache => cache
                .ContainingEntry(entry => entry
                    .WithKey(InformativeMessagesCacheKey)
                    .WithSlidingExpiration(TimeSpan.FromMinutes(30))
                    .WithValueOfType<IEnumerable<InformativeMessagesHeadingViewModel>>()))
            .AndAlso()
            .ShouldHave()
            .MemoryCache(cache => cache
                .ContainingEntry(entry => entry
                    .WithKey(UpCommingCoursesCacheKey)
                    .WithSlidingExpiration(TimeSpan.FromMinutes(30))
                    .WithValueOfType<IEnumerable<IndexSignUpForCourseViewModel>>()))
            .AndAlso()
            .ShouldReturn()
            .View(x => x.WithModelOfType(typeof(IndexUserViewModel)));
    }
}
