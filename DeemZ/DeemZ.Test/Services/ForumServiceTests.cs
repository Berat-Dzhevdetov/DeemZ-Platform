namespace DeemZ.Test.Services
{
    using System.Linq;
    using Xunit;
    using DeemZ.Models.FormModels.Forum;
    using DeemZ.Models.ViewModels.Forum;
    using System.Threading.Tasks;

    public class ForumServiceTests : BaseTestClass
    {
        public ForumServiceTests()
        {
            SetUpServices();
        }

        [Fact]
        public void CreatingTopicShouldAddItToTheDb()
        {
            var expectedTopicsCount = 1;

            SeedUser();

            forumService.CreateTopic(new CreateForumTopicFormModel()
            {
                Title = "Test",
                Description = "Test",
            }, testUserId);

            var actualForumTopicsCount = context.Forums.Count();

            Assert.Equal(expectedTopicsCount, actualForumTopicsCount);
        }

        [Fact]
        public async Task GettingAllTopicsShouldReturnTheCorrectTopics()
        {
            var expectedTopicId = await SeedForumTopic();

            var topics = forumService.GetAllTopics<ForumTopicsViewModel>();

            var actualTopicId = topics.First().Id;

            Assert.Equal(expectedTopicId, actualTopicId);
        }

        [Fact]
        public async Task GettingAllTopicsPaginatedShouldReturnTheCorrectTopics()
        {
            var expectedTopicsCount = 3;

            await SeedForumTopic();
            await SeedForumTopic(addUser: false);
            await SeedForumTopic(addUser: false);

            var topics = forumService.GetAllTopics<ForumTopicsViewModel>(1, 10);

            var actualTopicsCount = topics.Count();

            Assert.Equal(expectedTopicsCount, actualTopicsCount);
        }

        [Fact]
        public async Task GettingTopicByIdShouldReturnTheCorrectlyTopic()
        {
            var expectedTopicId = await SeedForumTopic();

            var topics = forumService.GetTopicById<ForumTopicsViewModel>(expectedTopicId);

            var actualTopicId = topics.Id;

            Assert.Equal(expectedTopicId, actualTopicId);
        }

        [Fact]
        public async Task GettingForumTopicsCountShouldReturnTheCorrectAmountOfTopics()
        {
            var expectedTopicsCount = 2;

            await SeedForumTopic();
            await SeedForumTopic(addUser: false);

            var actualTopicsCount = forumService.Count();

            Assert.Equal(expectedTopicsCount, actualTopicsCount);
        }

        [Fact]
        public async Task GettingTopicsByTitleShouldReturnTheCorrectTopics()
        {
            var expectedTopicsCount = 2;

            await SeedForumTopic();
            await SeedForumTopic(addUser: false);

            var actualTopicsCount = forumService.GetTopicsByTitleName<ForumTopicsViewModel>("Test").Count();

            Assert.Equal(expectedTopicsCount, actualTopicsCount);
        }

        [Fact]
        public async Task CreatingCommentShouldAddTheCommentToTheTopic()
        {
            var expectedCommentText = "Test-Comment";

            var topicId = await SeedForumTopic();

            await forumService.CreateComment(new AddCommentFormModel()
            {
                Text = expectedCommentText,
            }, topicId, testUserId);

            var topicComments = forumService.GetTopicById<TopicViewModel>(topicId).Comments;

            var actualCommentText = topicComments.First().Description;

            Assert.Equal(expectedCommentText, actualCommentText);
        }

        [Fact]
        public async Task GettingCommentByIdShouldReturnTheCorrectComment()
        {
            var expectedCommentText = "Test-Comment";

            var topicId = await SeedForumTopic();

            var commentId =await forumService.CreateComment(new AddCommentFormModel()
            {
                Text = expectedCommentText,
            }, topicId, testUserId);


            var actualCommentText = forumService.GetCommentById(commentId).Text;

            Assert.Equal(expectedCommentText, actualCommentText);
        }
    }
}
