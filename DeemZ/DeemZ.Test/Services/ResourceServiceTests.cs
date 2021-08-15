namespace DeemZ.Test.Services
{
    using Xunit;

    public class ResourceServiceTests : BaseTestClass
    {
        public ResourceServiceTests() => SetUpServices();

        [Fact]
        public void AddResourceToLectureShouldCorrectlyAddIt()
        {
            var courseId = SeedCourse();

            var lectureId = SeedLecture(courseId);
        }
    }
}