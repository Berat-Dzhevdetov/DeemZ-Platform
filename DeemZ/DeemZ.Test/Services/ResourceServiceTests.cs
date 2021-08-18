namespace DeemZ.Test.Services
{
    using System.Linq;
    using Xunit;
    using DeemZ.Models.FormModels.Resource;

    public class ResourceServiceTests : BaseTestClass
    {
        public ResourceServiceTests() => SetUpServices();

        [Fact]
        public void AddResourceToLectureShouldCorrectlyAddIt()
        {
            //Arange
            var expectedResourceCount = 1;

            var courseId = SeedCourse();

            var lectureId = SeedLecture(courseId);

            //Act
            resourceService.AddResourceToLecture(lectureId, "testId",
                new AddResourceFormModel { Name = "Test", Path = "Test-path", ResourceTypeId = "1" });

            var resourcesCountInDb = context.Resources.ToArray().Length;

            //Assert
            Assert.Equal(expectedResourceCount, resourcesCountInDb);
        }

        [Fact]
        public void GettingResourceByIdReturnsCorrectValueWhenResourceIsPresent()
        {
            //Arange
            var courseId = SeedCourse();

            var lectureId = SeedLecture(courseId);

            //Act
            var resourceId = resourceService.AddResourceToLecture(lectureId, "testId",
                new AddResourceFormModel { Name = "Test", Path = "Test-path", ResourceTypeId = "1" });

            //Asert
            Assert.True(resourceService.GetResourceById(resourceId));
        }

        [Fact]
        public void GetResourceByIdShouldReturnFalseWhenResourseDoenstExist() => Assert.False(resourceService.GetResourceById("invalid-id"));

        [Fact]
        public void DeletingAResourceTypeLinkShouldRemoveItFromTheDb()
        {
            var expectedCountFromDb = 0;

            var courseId = SeedCourse();

            var lectureId = SeedLecture(courseId);

            var resourceTypeId = SeedResourceTypes();

            //Act
            var resourceId = resourceService.AddResourceToLecture(lectureId, "testId",
                new AddResourceFormModel { Name = "Test", Path = "Test-path", ResourceTypeId = resourceTypeId });

            resourceService.Delete(resourceId);

            var actualCountFromDb = context.Resources.ToArray().Length;

            Assert.Equal(expectedCountFromDb, actualCountFromDb);
        }

        [Fact]
        public void GettingResourceByIdShouldReturnTHeCorrectResource()
        {
            var courseId = SeedCourse();

            var lectureId = SeedLecture(courseId);

            var resourceTypeId = SeedResourceTypes();

            //Act
            var resourceToAdd = new AddResourceFormModel
            {
                Name = "Test",
                Path = "Test-path",
                ResourceTypeId = resourceTypeId
            };

            var resourceId = resourceService.AddResourceToLecture(lectureId, "testId", resourceToAdd);

            var resourceFromService = resourceService.GetResourceById<AddResourceFormModel>(resourceId);

            Assert.Equal(resourceToAdd.Name, resourceFromService.Name);

        }

        [Fact]
        public void IsValidResourceTypeShouldReturnIfResourceIdisCorrect()
        {
            var resourceTypeId = SeedResourceTypes();

            var responseFromService = resourceService.IsValidResourceType(resourceTypeId);

            Assert.True(responseFromService);
        }

        [Fact]
        public void IsValidResourceTypeShouldNotReturnIfResourceIdisCorrect()
        {
            var resourceTypeId = "";

            var responseFromService = resourceService.IsValidResourceType(resourceTypeId);

            Assert.False(responseFromService);
        }

        [Fact]
        public void GetResourceTypesShouldRetunTheResourceTypes()
        {
            var resourceTypeId = SeedResourceTypes();

            var resourceTypeFromService = resourceService.GetResourceTypes<ResourceTypeFormModel>().First();

            Assert.Equal(resourceTypeId, resourceTypeFromService.Id);
        }
    }
}