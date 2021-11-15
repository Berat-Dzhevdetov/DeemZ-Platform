namespace DeemZ.Test.Services
{
    using System.Linq;
    using Xunit;
    using DeemZ.Models.FormModels.Resource;
    using System.Threading.Tasks;

    public class ResourceServiceTests : BaseTestClass
    {
        public ResourceServiceTests() => SetUpServices();

        [Fact]
        public async Task AddResourceToLectureShouldCorrectlyAddIt()
        {
            //Arange
            var expectedResourceCount = 1;

            var courseId = await SeedCourse();

            var lectureId = await SeedLecture(courseId);

            //Act
            await resourceService.AddResourceToLecture(lectureId, "testId",
                new AddResourceFormModel { Name = "Test", Path = "Test-path", ResourceTypeId = "1" });

            var resourcesCountInDb = context.Resources.ToArray().Length;

            //Assert
            Assert.Equal(expectedResourceCount, resourcesCountInDb);
        }

        [Fact]
        public async Task GettingResourceByIdReturnsCorrectValueWhenResourceIsPresent()
        {
            //Arange
            var courseId = await SeedCourse();

            var lectureId = await SeedLecture(courseId);

            //Act
            var resourceId = await resourceService.AddResourceToLecture(lectureId, "testId",
                new AddResourceFormModel { Name = "Test", Path = "Test-path", ResourceTypeId = "1" });

            //Asert
            Assert.True(resourceService.GetResourceById(resourceId));
        }

        [Fact]
        public void GetResourceByIdShouldReturnFalseWhenResourseDoenstExist() => Assert.False(resourceService.GetResourceById("invalid-id"));

        [Fact]
        public async Task GettingResourceByIdShouldReturnTheCorrectResource()
        {
            var courseId = await SeedCourse();

            var lectureId = await SeedLecture(courseId);

            var resourceTypeId = await SeedResourceTypes();

            //Act
            var resourceToAdd = new AddResourceFormModel
            {
                Name = "Test",
                Path = "Test-path",
                ResourceTypeId = resourceTypeId
            };

            var resourceId = await resourceService.AddResourceToLecture(lectureId, "testId", resourceToAdd);

            var resourceFromService = resourceService.GetResourceById<AddResourceFormModel>(resourceId);

            Assert.Equal(resourceToAdd.Name, resourceFromService.Name);

        }

        [Fact]
        public async Task IsValidResourceTypeShouldReturnIfResourceIdisCorrect()
        {
            var resourceTypeId = await SeedResourceTypes();

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
        public async Task GetResourceTypesShouldReturnTheResourceTypes()
        {
            var resourceTypeId = await SeedResourceTypes();

            var resourceTypeFromService = resourceService.GetResourceTypes<ResourceTypeFormModel>().First();

            Assert.Equal(resourceTypeId, resourceTypeFromService.Id);
        }

        [Fact]
        public async Task DeletingLectureResourcesShouldRemoveTheResourcesFromTheLecture()
        {
            var expectedCountFromDb = 0;

            var courseId = await SeedCourse();

            var lectureId = await SeedLecture(courseId);

            var resourceTypeId = await SeedResourceTypes();

            //Act
            await resourceService.AddResourceToLecture(lectureId, "testId",
                new AddResourceFormModel { Name = "Test", Path = "Test-path", ResourceTypeId = resourceTypeId });

            await resourceService.DeleteLectureResoureces(lectureId);

            var actualCountFromDb = context.Lectures.ToArray()[0].Resources.Count;

            Assert.Equal(expectedCountFromDb, actualCountFromDb);
        }

        [Fact]
        public async Task DeletingAResourceTypeLinkShouldRemoveItFromTheDb()
        {
            var expectedCountFromDb = 0;

            var courseId = await SeedCourse();

            var lectureId = await SeedLecture(courseId);

            var resourceTypeId = await SeedResourceTypes();

            //Act
            var resourceId = await resourceService.AddResourceToLecture(lectureId, "testId",
                new AddResourceFormModel { Name = "Test", Path = "Test-path", ResourceTypeId = resourceTypeId });

            await resourceService.Delete(resourceId);

            var actualCountFromDb = context.Resources.ToArray().Length;

            Assert.Equal(expectedCountFromDb, actualCountFromDb);
        }

        [Fact]
        public async Task DeletingLectureResourcesOnRemoteShouldRemoveTheResourcesFromTheLecture()
        {
            var expectedCountFromDb = 0;

            var courseId = await SeedCourse();

            var lectureId = await SeedLecture(courseId);

            var resourceTypeId =await SeedResourceTypes(false);

            //Act
            await resourceService.AddResourceToLecture(lectureId, "testId",
                new AddResourceFormModel { Name = "Test", Path = "Test-path", ResourceTypeId = resourceTypeId });

            await resourceService.DeleteLectureResoureces(lectureId);

            var actualCountFromDb = context.Lectures.ToArray()[0].Resources.Count;

            Assert.Equal(expectedCountFromDb, actualCountFromDb);
        }

        [Fact]
        public async Task DeletingARemoteResourceTypeLinkShouldRemoveItFromTheDb()
        {
            var expectedCountFromDb = 0;

            var courseId = await SeedCourse();

            var lectureId = await SeedLecture(courseId);

            var resourceTypeId =await SeedResourceTypes(false);

            //Act
            var resourceId = await resourceService.AddResourceToLecture(lectureId, "testId",
                new AddResourceFormModel { Name = "Test", Path = "Test-path", ResourceTypeId = resourceTypeId });

            await resourceService.Delete(resourceId);

            var actualCountFromDb = context.Resources.ToArray().Length;

            Assert.Equal(expectedCountFromDb, actualCountFromDb);
        }

        [Fact]
        public async Task DoesUserHavePermissionShouldReturnFalseIfTheUserDoesNotHavePermissionToViewTheResource()
        {
            //Arange
            var courseId = await SeedCourse();

            var lectureId = await SeedLecture(courseId);

            var username = "Bero";
            var userId = "test-user";
            await SeedUser(username, userId);

            //Act
            var resourceId = await resourceService.AddResourceToLecture(lectureId, "testId",
                new AddResourceFormModel { Name = "Test", Path = "Test-path", ResourceTypeId = "1" });

            //User needs to be enrolled in the course
            var result = resourceService.DoesUserHavePermissionToThisResource(resourceId, userId);

            Assert.False(result);
        }

        [Fact]
        public async Task DoesUserHavePermissionShouldReturnTrueIfTheUserHasPermissionToViewTheResource()
        {
            //Arange
            var courseId = await SeedCourse();

            var lectureId = await SeedLecture(courseId);

            var username = "Bero";
            var userId = "test-user";
            await SeedUser(username, userId);

            //Act
            var resourceId = await resourceService.AddResourceToLecture(lectureId, "testId",
                new AddResourceFormModel { Name = "Test", Path = "Test-path", ResourceTypeId = "1" });

            //User needs to be enrolled in the course
            SeedUserCourse(courseId, userId);
            
            var result = resourceService.DoesUserHavePermissionToThisResource(resourceId, userId);
            
            Assert.True(result);
        }
    }
}